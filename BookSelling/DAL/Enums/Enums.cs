using System.ComponentModel;

namespace DAL.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
    public enum OfferStatus
    {
        [Description("Book is available")]
        Available = 0,
        [Description("Order requested successfully. Awaiting from seller for acceptance of the order")]
        Requested,
        [Description("Order has been accepted")]
        Accepted
    }
}
