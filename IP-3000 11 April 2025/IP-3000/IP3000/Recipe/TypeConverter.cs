using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3000.Recipe
{
    public class PointRecipeObjectConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            List<PointRecipe> pointRecipes = value as List<PointRecipe>;
            if (pointRecipes == null)
                return "-";

            return string.Join(", ", pointRecipes.Select(m => m.PosName.ToString()));
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            List<PointRecipe> pointRecipes = value as List<PointRecipe>;
            //Items members = value as Items;
            if (pointRecipes != null)
            {
                foreach (PointRecipe pointRecipe in pointRecipes)
                {
                    if (pointRecipe.PosName != null)
                    {
                        list.Add(new DistanceDescriptor(pointRecipe, list.Count));
                    }
                }
            }
            return new PropertyDescriptorCollection(list.ToArray());
        }

        private class DistanceDescriptor : SimplePropertyDescriptor
        {
            public DistanceDescriptor(PointRecipe pointRecipe, int index)
                : base(pointRecipe.GetType(), index.ToString(), typeof(string))
            {
                PointRecipe1 = pointRecipe;
            }

            public PointRecipe PointRecipe1 { get; private set; }

            public override object GetValue(object component)
            {
                //return Member.Name;
                return PointRecipe1;
            }

            public override void SetValue(object component, object value)
            {
                //Member.Name = (string)value;
                PointRecipe1 = value as PointRecipe;
            }
        }
    }

    public class OneSideRecipeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            List<OneSideRecipe> members = value as List<OneSideRecipe>;
            if (members == null)
                return "-";

            return string.Join(", ", members.Select(m => m.Name));
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            List<OneSideRecipe> members = value as List<OneSideRecipe>;
            //Items members = value as Items;
            if (members != null)
            {
                foreach (OneSideRecipe member in members)
                {
                    if (member.Name != null)
                    {
                        list.Add(new MemberDescriptor(member, list.Count));
                    }
                }
            }
            return new PropertyDescriptorCollection(list.ToArray());
        }

        private class MemberDescriptor : SimplePropertyDescriptor
        {
            public MemberDescriptor(OneSideRecipe oneSideRecipe, int index)
                : base(oneSideRecipe.GetType(), index.ToString(), typeof(string))
            {
                OneSideRecipe1 = oneSideRecipe;
            }

            public OneSideRecipe OneSideRecipe1 { get; private set; }

            public override object GetValue(object component)
            {
                //return Member.Name;
                return OneSideRecipe1;
            }

            public override void SetValue(object component, object value)
            {
                //Member.Name = (string)value;
                OneSideRecipe1 = value as OneSideRecipe;
            }
        }
    }
}
