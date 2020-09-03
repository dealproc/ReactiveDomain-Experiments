namespace RD.Core.ValueObjects {
    public interface IValueObject {
        object ToPrimitiveType();
        string ToString();
    }
}