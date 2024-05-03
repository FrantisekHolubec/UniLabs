```
public class TimeExample : MonoBehaviour
{
    [SerializeField]
    private UDateTime _ExampleDate;

    [PropertySpace]
    [SerializeField]
    private UTimeSpan _ExampleTime1;

    [TimeSpanDrawerSettings(TimeUnit.Minutes)]
    [SerializeField]
    private UTimeSpan _ExampleTime2;

    [TimeSpanDrawerSettings(TimeUnit.Hours, TimeUnit.Minutes)]
    [SerializeField]
    private UTimeSpan _ExampleTime3;

    [TimeSpanDrawerSettings(TimeUnit.Days, TimeUnit.Days)]
    [SerializeField]
    private UTimeSpan _ExampleTime4;

    [PropertySpace]
    [TimeSpanDrawerSettings(TimeUnit.Minutes)]
    [TimeSpanRange("@TimeSpan.FromMinutes(10)")]
    [SerializeField]
    private UTimeSpan _ExampleTimeRange1;

    [TimeSpanDrawerSettings(TimeUnit.Minutes)]
    [TimeSpanRange("@TimeSpan.FromMinutes(10)", true)]
    [SerializeField]
    private UTimeSpan _ExampleTimeRange2;

    [PropertySpace]
    [TimeSpanRange("@TimeSpan.FromMinutes(10)")]
    [TimeSpanDrawerSettings(TimeUnit.Minutes)]
    [SerializeField]
    private UTimeSpanRange _ExampleTimeRangeMinMax;

    [TimeSpanRange("@TimeSpan.FromMinutes(10)", true)]
    [TimeSpanDrawerSettings(TimeUnit.Minutes)]
    [SerializeField]
    private UTimeSpanRange _ExampleTimeRangeMinMax2;
}
```