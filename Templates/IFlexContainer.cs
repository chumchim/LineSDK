namespace LineSDK.Templates;

/// <summary>
/// Base interface สำหรับ Flex Container (Bubble หรือ Carousel)
/// </summary>
public interface IFlexContainer
{
    string Type { get; }
    object ToJson();
}

/// <summary>
/// Base interface สำหรับ Flex Component
/// </summary>
public interface IFlexComponent
{
    string Type { get; }
    object ToJson();
}
