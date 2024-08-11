<h1>Mẹo .NET👋</h1>

- Tìm index trong vòng lập foreach

<h3>1. Tìm index trong vòng lập foreach</h3>

```c#
IEnummerable<T> Object = new IEnummerable<T>();

foreach(var (value, index) in Object.Select((v, i)=>(v, i)))
{
    // value: là đối tượng

    // index: vị trí đối tượng
}
```

⭐️ From [Ruộng](https://github.com/liuvt)
