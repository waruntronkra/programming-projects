using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
namespace IP3000_Control.DL
{
  public static  class CenterMethod
    {
        public static List<string> DeviceList;
        public static void Initial_Heatmap(HTuple DLModelHandle, string HeatmapMethod, ref HDict HeatmapParam)
        {
            try
            {
                //*Set the target class ID or[] to show the heatmap for the classified class.

                if (HeatmapParam != null)
                {
                    HeatmapParam.ClearHandle();
                    HeatmapParam.Dispose();
                }
                HeatmapParam = null;
                HeatmapParam = new HDict();
                HeatmapParam.CreateDict();

                if (HeatmapMethod == "heatmap_grad_cam")
                {
                    //* Set generic parameters for operator heatmap.
                    HeatmapParam.SetDictTuple("use_conv_only", "false");
                    HeatmapParam.SetDictTuple("scaling", "scale_after_relu");
                }
                else if (HeatmapMethod == "heatmap_confidence_based")
                {
                    HTuple TargetClassID = new HTuple();
                    HTuple FeatureSize, SamplingSize;
                    //* Set target class ID.
                    HeatmapParam.SetDictTuple("target_class_id", TargetClassID);
                    //* Set the feature size and the sampling size for the
                    //* confidence based approach.
                    FeatureSize = 30;
                    SamplingSize = 10;
                    HeatmapParam.SetDictTuple("feature_size", FeatureSize);
                    HeatmapParam.SetDictTuple("sampling_size", SamplingSize);
                }

                //*Heatmaps are displayed in sequence, hence set batch size to 1.
                //HOperatorSet.SetDlModelParam(DLModelHandle, "batch_size", 1);

            }
            catch (Exception ex)
            {
                ex = null;
            }
        }
        public static void Add_ColorMap_To_Image(HImage GrayValueImage, HImage Image, HTuple HeatmapColorScheme, ref HImage ColoredImage)
        {
            try
            {
                // *This procedure adds a gray-value image to a RGB image with a chosen colormap.
                string ImageType = GrayValueImage.GetImageType();
                //* The image LUT needs a byte image. Rescale real images.
                if (ImageType == "real")
                {
                    Scale_Image_Range(GrayValueImage, ref GrayValueImage, 0, 1);
                    GrayValueImage = GrayValueImage.ConvertImageType("byte");
                }
                HImage RGBValueImage = null;
                //*** Apply the chosen color scheme on the gray value.
                Apply_Colorscheme_On_Gray_Value_Image(GrayValueImage, HeatmapColorScheme, ref RGBValueImage);


                //*Convert input image to byte image for visualization.
                HImage Channels = Image.ImageToChannels();
                int NumChannels = Image.CountChannels();
                HImage ChannelsScaled = new HImage();
                ChannelsScaled.GenEmptyObj();

                for (int ChannelIndex = 1; ChannelIndex <= NumChannels; ChannelIndex++)
                {

                    HImage Channel = Channels.SelectObj(ChannelIndex);
                    HImage ChannelScaled = new HImage();
                    Channel.MinMaxGray(Channel, 0, out HTuple ChannelMin, out HTuple ChannelMax, out HTuple Range);
                    Scale_Image_Range(Channel, ref ChannelScaled, ChannelMin, ChannelMax);
                    HImage ChannelScaledByte = ChannelScaled.ConvertImageType("byte");

                    ChannelsScaled = ChannelsScaled.ConcatObj(ChannelScaledByte);

                }

                HImage ImageByte = ChannelsScaled.ChannelsToImage();

                //Note that ImageByte needs to have the same number of channels as
                //* RGBValueImage to display colormap image correctly.
                NumChannels = ImageByte.CountChannels();

                if (NumChannels != 3)
                {
                    //*Just take the first channel and use this to generate
                    //*an image with 3 channels for visualization.
                    HImage ImageByteR = ImageByte.AccessChannel(1);
                    HImage ImageByteG = ImageByteR.CopyImage();
                    HImage ImageByteB = ImageByteR.CopyImage();
                    ImageByte = ImageByteR.Compose3(ImageByteG, ImageByteB);
                }

                RGBValueImage = ImageByte.AddImage(RGBValueImage, 0.5, 0);
                ColoredImage = RGBValueImage;

            }
            catch (Exception ex)
            {
                ex = null;
            }

        }
        public static void Scale_Image_Range(HImage Image, ref HImage ImageScaled, HTuple Min, HTuple Max)
        {
            try
            {
                HTuple LowerLimit, UpperLimit, Mult, Add;
                if (Min.Length == 2)
                {
                    LowerLimit = Min[1];
                    Min = Min[0];
                }
                else
                {
                    LowerLimit = 0.0;
                }
                if (Max.Length == 2)
                {
                    UpperLimit = Max[1];
                    Max = Max[0];
                }
                else
                {
                    UpperLimit = 255.0;
                }

                //*Calculate scaling parameters. Only scale if the scaling range is not zero.
                HTuple DiffMaxMin = (Max - Min).TupleAbs();
                if (DiffMaxMin.TupleLess(1.0E-6).TupleNot())
                {
                    Mult = (UpperLimit - LowerLimit).TupleReal() / (Max - Min);
                    Add = -Mult * Min + LowerLimit;
                    //* Scale image.
                    Image = Image.ScaleImage(Mult, Add);
                }

                //*Clip gray values if necessary.
                //* This must be done for each image and channel separately.
                ImageScaled = new HImage();
                ImageScaled.GenEmptyObj();

                int NumImages = Image.CountObj();
                HImage ImageSelectedScaled = new HImage();
                for (int ImageIndex = 1; ImageIndex <= NumImages; ImageIndex++)
                {
                    HImage ImageSelected = new HImage(Image.SelectObj(ImageIndex));

                    int Channels = ImageSelected.CountChannels();

                    for (int ChannelIndex = 1; ChannelIndex <= Channels; ChannelIndex++)
                    {
                        HImage SelectedChannel = ImageSelected.AccessChannel(ChannelIndex);
                        HTuple MinGray, MaxGray, Range;
                        SelectedChannel.MinMaxGray(SelectedChannel, 0, out MinGray, out MaxGray, out Range);
                        HRegion LowerRegion = SelectedChannel.Threshold(MinGray.TupleMin2(LowerLimit), LowerLimit);
                        HRegion UpperRegion = SelectedChannel.Threshold(UpperLimit, UpperLimit.TupleMax2(MaxGray));
                        SelectedChannel = LowerRegion.PaintRegion(SelectedChannel, LowerLimit, "fill");
                        SelectedChannel = UpperRegion.PaintRegion(SelectedChannel, UpperLimit, "fill");

                        if (ChannelIndex == 1)
                            ImageSelectedScaled = SelectedChannel.CopyObj(1, 1);
                        else
                            ImageSelectedScaled = ImageSelectedScaled.AppendChannel(ImageSelectedScaled);

                    }// end loop for
                    ImageScaled = ImageScaled.ConcatObj(ImageSelectedScaled);
                }// end loop for
            }
            catch (Exception ex)
            {
                ex = null;
            }

        }
        public static void Apply_Colorscheme_On_Gray_Value_Image(HImage InputImage, HTuple Schema, ref HImage ResultImage)
        {
            try
            {
                //*This procedure generates an RGB ResultImage for a grey-value InputImage.
                //*according to the Schema.
                HTuple X = HTuple.TupleGenSequence(0, 255, 1);
                HOperatorSet.TupleGenConst(256, 0, out HTuple Low);
                HOperatorSet.TupleGenConst(256, 255, out HTuple High);
                HTuple R = null, G = null, B = null;

                if (Schema == "jet")
                {
                    //*Scheme Jet: from blue to red
                    HTuple OffR = 3.0 * 64.0;
                    HTuple OffG = 2.0 * 64.0;
                    HTuple OffB = 64.0;
                    HTuple A1 = -4.0;
                    HTuple A0 = 255.0 + 128.0;

                    R = ((X - OffR).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    G = ((X - OffG).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    B = ((X - OffB).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);

                }
                else if (Schema == "inverse_jet")
                {
                    //* Scheme InvJet: from red to blue.
                    HTuple OffR = 64;
                    HTuple OffG = 2 * 64;
                    HTuple OffB = 3 * 64;
                    HTuple A1 = -4.0;
                    HTuple A0 = 255.0 + 128.0;
                    R = ((X - OffR).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    G = ((X - OffG).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);
                    B = ((X - OffB).TupleAbs() * A1 + A0).TupleMax2(Low).TupleMin2(High);

                }
                else if (Schema == "hot")
                {
                    //* Scheme Hot.
                    HTuple A1 = 3.0;
                    HTuple A0R = 0.0;
                    HTuple A0G = 1.0 / 3.0 * A1 * 255.0;
                    HTuple A0B = 2.0 / 3.0 * A1 * 255.0;
                    R = (X.TupleAbs() * A1 - A0R).TupleMax2(Low).TupleMin2(High);
                    G = (X.TupleAbs() * A1 - A0G).TupleMax2(Low).TupleMin2(High);
                    B = (X.TupleAbs() * A1 - A0B).TupleMax2(Low).TupleMin2(High);
                }
                else if (Schema == "inverse_hot")
                {
                    //* Scheme Inverse Hot.
                    HTuple A1 = -3.0;
                    HTuple A0R = A1 * 255.0;
                    HTuple A0G = 2.0 / 3.0 * A1 * 255.0;
                    HTuple A0B = 1.0 / 3.0 * A1 * 255.0;
                    R = (X.TupleAbs() * A1 - A0R).TupleMax2(Low).TupleMin2(High);
                    G = (X.TupleAbs() * A1 - A0G).TupleMax2(Low).TupleMin2(High);
                    B = (X.TupleAbs() * A1 - A0B).TupleMax2(Low).TupleMin2(High);
                }
                


                HImage ImageR = InputImage.LutTrans(R);
                HImage ImageG = InputImage.LutTrans(G);
                HImage ImageB = InputImage.LutTrans(B);

                ResultImage = ImageR.Compose3(ImageG, ImageB);


            }
            catch (Exception ex)
            {
                ex = null;
            }

        }
        public static void GetComputeInfo()
        {
            try
            {
                HTuple Information = new HTuple();
                HOperatorSet.GetSystem("cuda_devices", out Information);
                DeviceList = new List<string>();
                HTuple DlInfo = null;
                HOperatorSet.QueryAvailableDlDevices(new HTuple(), new HTuple(), out DlInfo);
                for (int i = 0; i < DlInfo.Length; i++)
                {
                    HTuple ID = 0;
                    HTuple Name = "";
                    HTuple Type = "";
                    HTuple ai_accelerator_interface = "";
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "name", out Name);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "type", out Type);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "ai_accelerator_interface", out ai_accelerator_interface);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "id", out ID);

                    string Value = "";
                    if (ai_accelerator_interface == "none")
                        Value = Name + " [" + Type.S.ToUpper() + ":" + ID + "]";
                    else
                        Value = Name + " [" + Type.S.ToUpper() + ":" + ID + ":" + ai_accelerator_interface.S.ToUpper() + "]";

                    DeviceList.Add(Value);

                }

            }
            catch (Exception ex)
            {
                ex = null;
            }
        }
        public static void Set_Device(string SelectName,ref HDlModel ModelHandle)
        {
            try
            {
                
                HTuple Information = new HTuple();
                HOperatorSet.GetSystem("cuda_devices", out Information);
                DeviceList = new List<string>();
                HTuple DlInfo = null;
                HOperatorSet.QueryAvailableDlDevices(new HTuple(), new HTuple(), out DlInfo);
                for (int i = 0; i < DlInfo.Length; i++)
                {
                    HTuple ID = 0;
                    HTuple Name = "";
                    HTuple Type = "";
                    HTuple ai_accelerator_interface = "";
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "name", out Name);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "type", out Type);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "ai_accelerator_interface", out ai_accelerator_interface);
                    HOperatorSet.GetDlDeviceParam(DlInfo[i], "id", out ID);

                    string Value = "";
                    if (ai_accelerator_interface == "none")
                        Value = Name + " [" + Type.S.ToUpper() + ":" + ID + "]";
                    else
                        Value = Name + " [" + Type.S.ToUpper() + ":" + ID + ":" + ai_accelerator_interface.S.ToUpper() + "]";

                    if(SelectName== Value)
                    {
                        ModelHandle.SetDlModelParam("device", DlInfo[i].H);
                        
break;
                    }

                }


            }
            catch (Exception ex)
            {
                ex = null;

            }
        }
        

    }
}
