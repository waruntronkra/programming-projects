using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3000.Recipe
{
    public enum ProductSide
    {
        Top,
        Bottom,
        PCBA
    }

    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class ListRecipe
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }
        public string Customer { get; set; }

        [TypeConverter(typeof(OneSideRecipeConverter))]
        public List<OneSideRecipe> OneSideRecipes { get; set; } = new List<OneSideRecipe>();
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OneSideRecipe
    {
        #region _private variables and Properties

        public ProductSide Side { get; set; }
        public string Name { get; set; } //Name of the member used to uniquely identify it
        public string PathFailImage { get; set; } //Size of the member (ex. 2x4)
        public string PathGoodImage { get; set; } //Grade of the wood used to make the member
        public string DLProgramName { get; set; }
        public string MotionPrgName { get; set; }

        [TypeConverter(typeof(PointRecipeObjectConverter))]
        public List<PointRecipe> ListPointRecipe { get; set; } //Length of the member

        #endregion
        /*Constructors and Methods not shown*/
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PointRecipe
    {
        private double areaThreshold = 1000.0;
        private double clsThreshold = 0.3;
        private double segThreshold = 0.5;
        private string posName = "Pos";

        public double AreaThreshold
        {
            get { return areaThreshold; }
            set { areaThreshold = value; }
        }

        public double ClsThreshold
        {
            get { return clsThreshold; }
            set { clsThreshold = value; }
        }

        public double SegmentThreshold
        {
            get { return segThreshold; }
            set { segThreshold = value; }
        }

        public string PosName
        {
            get { return posName; }
            set { posName = value; }
        }
    }

}
