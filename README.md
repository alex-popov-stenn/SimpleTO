# .NET Performance Benchmarks
This repository contains benchmarks for various situations we usually meet when developing libraries or business features

# Allocation

```csharp
[Benchmark]
public void NewList()
{
    var list = new List<int>(Records);
}

[Benchmark]
public void NewArray()
{
    var array = new int[Records];
}

[Benchmark]
public void NewDictionary()
{
    var dictionary = new Dictionary<int, int>(Records);
}

[Benchmark]
public void NewHashSet()
{
    var hashSet = new HashSet<int>(Records);
}

[Benchmark]
public void NewEnumerable()
{
    var enumerable = Enumerable.Range(0, Records);
}
```

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|        Method | Records |             Mean |          Error |         StdDev |           Median |     Gen0 |     Gen1 |     Gen2 |  Allocated |
|-------------- |-------- |-----------------:|---------------:|---------------:|-----------------:|---------:|---------:|---------:|-----------:|
|       **NewList** |       **5** |         **7.569 ns** |      **0.1548 ns** |      **0.1448 ns** |         **7.509 ns** |   **0.0383** |        **-** |        **-** |       **80 B** |
|      NewArray |       5 |         2.867 ns |      0.0544 ns |      0.0454 ns |         2.876 ns |   0.0230 |        - |        - |       48 B |
| NewDictionary |       5 |        20.673 ns |      0.1505 ns |      0.1408 ns |        20.721 ns |   0.1301 |        - |        - |      272 B |
|    NewHashSet |       5 |        17.872 ns |      0.1084 ns |      0.0846 ns |        17.857 ns |   0.1109 |        - |        - |      232 B |
| NewEnumerable |       5 |         5.268 ns |      0.0283 ns |      0.0251 ns |         5.270 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |     **100** |        **18.963 ns** |      **0.0338 ns** |      **0.0316 ns** |        **18.965 ns** |   **0.2180** |        **-** |        **-** |      **456 B** |
|      NewArray |     100 |        13.703 ns |      0.0663 ns |      0.0518 ns |        13.693 ns |   0.2027 |        - |        - |      424 B |
| NewDictionary |     100 |        82.503 ns |      0.7354 ns |      0.6141 ns |        82.325 ns |   1.0858 |        - |        - |     2272 B |
|    NewHashSet |     100 |        76.037 ns |      0.2525 ns |      0.2109 ns |        76.037 ns |   0.8756 |        - |        - |     1832 B |
| NewEnumerable |     100 |         5.152 ns |      0.0721 ns |      0.0709 ns |         5.136 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |     **500** |        **65.144 ns** |      **0.8997 ns** |      **0.7024 ns** |        **64.975 ns** |   **0.9823** |        **-** |        **-** |     **2056 B** |
|      NewArray |     500 |        56.698 ns |      0.0663 ns |      0.0588 ns |        56.675 ns |   0.9671 |        - |        - |     2024 B |
| NewDictionary |     500 |       327.046 ns |      5.7616 ns |      7.2866 ns |       323.929 ns |   5.0378 |        - |        - |    10552 B |
|    NewHashSet |     500 |       268.156 ns |      1.7987 ns |      1.5020 ns |       268.175 ns |   4.0321 |        - |        - |     8456 B |
| NewEnumerable |     500 |         5.269 ns |      0.0252 ns |      0.0197 ns |         5.270 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |    **1000** |       **134.645 ns** |      **4.1141 ns** |     **12.0011 ns** |       **133.070 ns** |   **1.9379** |        **-** |        **-** |     **4056 B** |
|      NewArray |    1000 |       117.319 ns |      2.2918 ns |      3.7654 ns |       118.688 ns |   1.9228 |        - |        - |     4024 B |
| NewDictionary |    1000 |       679.764 ns |     24.7701 ns |     69.4581 ns |       660.735 ns |  10.5257 |        - |        - |    22192 B |
|    NewHashSet |    1000 |       568.598 ns |     18.2943 ns |     53.0751 ns |       548.425 ns |   8.4743 |        - |        - |    17768 B |
| NewEnumerable |    1000 |         7.239 ns |      0.4350 ns |      1.2825 ns |         7.734 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |   **10000** |     **1,172.403 ns** |     **35.9203 ns** |    **104.7812 ns** |     **1,167.119 ns** |  **18.8675** |        **-** |        **-** |    **40056 B** |
|      NewArray |   10000 |       970.245 ns |     19.4235 ns |     44.2372 ns |       958.123 ns |  18.8675 |        - |        - |    40024 B |
| NewDictionary |   10000 |     9,824.735 ns |    212.0709 ns |    605.0503 ns |     9,664.417 ns |  49.9878 |  49.9878 |  49.9878 |   202209 B |
|    NewHashSet |   10000 |     8,718.205 ns |    245.3007 ns |    711.6619 ns |     8,556.238 ns |  38.4521 |  38.4521 |  38.4521 |   161781 B |
| NewEnumerable |   10000 |         5.687 ns |      0.1716 ns |      0.4897 ns |         5.559 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |   **50000** |     **6,956.950 ns** |    **137.4558 ns** |    **205.7374 ns** |     **6,960.099 ns** |  **62.4924** |  **62.4924** |  **62.4924** |   **200076 B** |
|      NewArray |   50000 |     7,026.850 ns |    140.4490 ns |    299.3082 ns |     6,976.780 ns |  62.4924 |  62.4924 |  62.4924 |   200044 B |
| NewDictionary |   50000 |   207,767.931 ns |  4,777.6618 ns | 14,087.0435 ns |   208,657.678 ns | 285.6445 | 285.6445 | 285.6445 |  1047445 B |
|    NewHashSet |   50000 |    26,474.433 ns |    465.1552 ns |    710.3411 ns |    26,431.931 ns | 249.9695 | 249.9695 | 249.9695 |   837976 B |
| NewEnumerable |   50000 |         5.530 ns |      0.1450 ns |      0.2759 ns |         5.459 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** |  **500000** |    **41,304.410 ns** |    **151.9788 ns** |    **118.6550 ns** |    **41,297.043 ns** | **499.9390** | **499.9390** | **499.9390** |  **2000218 B** |
|      NewArray |  500000 |    48,600.851 ns |    964.0528 ns |  2,606.3727 ns |    48,820.947 ns | 499.9390 | 499.9390 | 499.9390 |  2000151 B |
| NewDictionary |  500000 | 1,449,880.163 ns | 30,110.3898 ns | 87,355.7007 ns | 1,437,187.891 ns | 378.9063 | 378.9063 | 378.9063 | 11215240 B |
|    NewHashSet |  500000 | 1,134,630.679 ns | 22,635.7406 ns | 62,723.5307 ns | 1,126,885.938 ns | 351.5625 | 349.6094 | 349.6094 |  8972027 B |
| NewEnumerable |  500000 |         5.224 ns |      0.0223 ns |      0.0198 ns |         5.223 ns |   0.0191 |        - |        - |       40 B |
|       **NewList** | **1000000** |    **78,618.241 ns** |    **400.7121 ns** |    **374.8264 ns** |    **78,702.222 ns** | **999.8779** | **999.8779** | **999.8779** |  **4000391 B** |
|      NewArray | 1000000 |    77,895.227 ns |    966.9746 ns |    807.4676 ns |    77,872.693 ns | 999.8779 | 999.8779 | 999.8779 |  4000352 B |
| NewDictionary | 1000000 | 1,936,888.525 ns | 33,619.0190 ns | 33,018.3719 ns | 1,939,648.926 ns | 630.8594 | 626.9531 | 626.9531 | 23256936 B |
|    NewHashSet | 1000000 | 1,601,207.063 ns | 31,293.9806 ns | 58,005.3996 ns | 1,584,240.625 ns | 500.0000 | 498.0469 | 498.0469 | 18604465 B |
| NewEnumerable | 1000000 |         6.982 ns |      0.3755 ns |      1.1072 ns |         6.772 ns |   0.0191 |        - |        - |       40 B |

*Observations*:
1. Use arrays when you don't expect collection size changes
2. Use Enumerable when you pursue to avoid immediate memory allocations

# Iterating

```csharp
[Benchmark]
public void ListFor()
{
    for (int i = 0; i < list.Count; i++)
    {
    }
}

[Benchmark]
public void ListForeach()
{
    foreach (var item in list)
    {
    }
}

[Benchmark]
public void ListAsGenericEnumerable()
{
    foreach (var item in list.AsEnumerable())
    {
    }
}

[Benchmark]
public void ListAsPlainEnumerable()
{
    IEnumerable enumerable = list;

    foreach (var item in enumerable)
    {
    }
}

[Benchmark]
public void ArrayFor()
{
    for (int i = 0; i < array.Length; i++)
    {
    }
}

[Benchmark]
public void ArrayForeach()
{
    foreach (var item in array)
    {
    }
}

[Benchmark]
public void DictionaryForeach()
{
    foreach (var item in dictionary)
    {
    }
}

[Benchmark]
public void HashSetForeach()
{
    foreach (var item in hashSet)
    {
    }
}

[Benchmark]
public void GenericEnumerableForeach()
{
    foreach (var item in enumerable)
    {
    }
}

[Benchmark]
public void PlainEnumerableForeach()
{
    var e = (IEnumerable)enumerable;

    foreach (var item in e)
    {
    }
}
```

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|                   Method | Records |              Mean |           Error |          StdDev |       Gen0 |  Allocated |
|------------------------- |-------- |------------------:|----------------:|----------------:|-----------:|-----------:|
|                  **ListFor** |       **5** |         **0.9875 ns** |       **0.0411 ns** |       **0.0384 ns** |          **-** |          **-** |
|              ListForeach |       5 |         2.7701 ns |       0.0158 ns |       0.0148 ns |          - |          - |
|  ListAsGenericEnumerable |       5 |        45.2056 ns |       0.1784 ns |       0.1669 ns |     0.0191 |       40 B |
|    ListAsPlainEnumerable |       5 |        60.4338 ns |       1.2417 ns |       1.4300 ns |     0.0764 |      160 B |
|                 ArrayFor |       5 |         1.0322 ns |       0.0479 ns |       0.0492 ns |          - |          - |
|             ArrayForeach |       5 |         0.9427 ns |       0.0203 ns |       0.0170 ns |          - |          - |
|        DictionaryForeach |       5 |        19.2364 ns |       0.2173 ns |       0.2032 ns |          - |          - |
|           HashSetForeach |       5 |        14.0138 ns |       0.1463 ns |       0.1297 ns |          - |          - |
| GenericEnumerableForeach |       5 |        29.1025 ns |       0.2364 ns |       0.2212 ns |     0.0191 |       40 B |
|   PlainEnumerableForeach |       5 |        42.6809 ns |       0.8615 ns |       0.8846 ns |     0.0765 |      160 B |
|                  **ListFor** |     **100** |        **29.7365 ns** |       **0.1359 ns** |       **0.1135 ns** |          **-** |          **-** |
|              ListForeach |     100 |        43.0324 ns |       0.2257 ns |       0.1884 ns |          - |          - |
|  ListAsGenericEnumerable |     100 |       651.0318 ns |       2.7561 ns |       2.5781 ns |     0.0191 |       40 B |
|    ListAsPlainEnumerable |     100 |       905.5293 ns |      17.1402 ns |      23.4617 ns |     1.1663 |     2440 B |
|                 ArrayFor |     100 |        29.5245 ns |       0.0330 ns |       0.0293 ns |          - |          - |
|             ArrayForeach |     100 |        29.6491 ns |       0.0265 ns |       0.0235 ns |          - |          - |
|        DictionaryForeach |     100 |       293.5200 ns |       3.1482 ns |       2.9448 ns |          - |          - |
|           HashSetForeach |     100 |       247.6698 ns |       3.9599 ns |       3.7041 ns |          - |          - |
| GenericEnumerableForeach |     100 |       371.7560 ns |       0.4780 ns |       0.4472 ns |     0.0191 |       40 B |
|   PlainEnumerableForeach |     100 |       633.3775 ns |       2.3230 ns |       1.9398 ns |     1.1663 |     2440 B |
|                  **ListFor** |     **500** |       **121.3294 ns** |       **0.1781 ns** |       **0.1487 ns** |          **-** |          **-** |
|              ListForeach |     500 |       188.2064 ns |       0.8668 ns |       0.7684 ns |          - |          - |
|  ListAsGenericEnumerable |     500 |     3,164.6643 ns |       2.7722 ns |       2.3149 ns |     0.0191 |       40 B |
|    ListAsPlainEnumerable |     500 |     4,317.0388 ns |      21.7741 ns |      16.9998 ns |     5.7526 |    12040 B |
|                 ArrayFor |     500 |       122.3381 ns |       1.3803 ns |       1.2912 ns |          - |          - |
|             ArrayForeach |     500 |       121.1332 ns |       0.0954 ns |       0.0796 ns |          - |          - |
|        DictionaryForeach |     500 |     1,251.2905 ns |       5.2859 ns |       4.6858 ns |          - |          - |
|           HashSetForeach |     500 |     1,180.3528 ns |       1.7615 ns |       1.5615 ns |          - |          - |
| GenericEnumerableForeach |     500 |     1,788.3952 ns |      14.4872 ns |      13.5514 ns |     0.0191 |       40 B |
|   PlainEnumerableForeach |     500 |     2,982.2553 ns |       9.5279 ns |       7.9562 ns |     5.7564 |    12040 B |
|                  **ListFor** |    **1000** |       **236.7877 ns** |       **0.0449 ns** |       **0.0350 ns** |          **-** |          **-** |
|              ListForeach |    1000 |       476.4301 ns |       6.5970 ns |       6.1709 ns |          - |          - |
|  ListAsGenericEnumerable |    1000 |     6,325.7206 ns |      12.4536 ns |      10.3993 ns |     0.0153 |       40 B |
|    ListAsPlainEnumerable |    1000 |     8,364.6530 ns |      30.3466 ns |      23.6927 ns |    11.4899 |    24040 B |
|                 ArrayFor |    1000 |       236.9788 ns |       0.3847 ns |       0.3004 ns |          - |          - |
|             ArrayForeach |    1000 |       236.8346 ns |       0.0716 ns |       0.0635 ns |          - |          - |
|        DictionaryForeach |    1000 |     2,813.1126 ns |       1.5346 ns |       1.4354 ns |          - |          - |
|           HashSetForeach |    1000 |     2,371.9066 ns |       3.5742 ns |       2.9846 ns |          - |          - |
| GenericEnumerableForeach |    1000 |     3,554.4851 ns |      42.2219 ns |      39.4944 ns |     0.0191 |       40 B |
|   PlainEnumerableForeach |    1000 |     5,957.9892 ns |      26.7502 ns |      22.3376 ns |    11.4899 |    24040 B |
|                  **ListFor** |   **10000** |     **2,318.1597 ns** |       **0.2457 ns** |       **0.2178 ns** |          **-** |          **-** |
|              ListForeach |   10000 |     4,656.1289 ns |       3.0105 ns |       2.3504 ns |          - |          - |
|  ListAsGenericEnumerable |   10000 |    60,785.3812 ns |     432.6277 ns |     383.5132 ns |          - |       40 B |
|    ListAsPlainEnumerable |   10000 |    80,878.9325 ns |     178.0718 ns |     148.6980 ns |   114.7461 |   240040 B |
|                 ArrayFor |   10000 |     2,318.2168 ns |       0.3325 ns |       0.2947 ns |          - |          - |
|             ArrayForeach |   10000 |     2,325.8036 ns |      13.7213 ns |      11.4579 ns |          - |          - |
|        DictionaryForeach |   10000 |    24,963.8365 ns |     248.6426 ns |     220.4152 ns |          - |          - |
|           HashSetForeach |   10000 |    23,606.3993 ns |      15.5296 ns |      14.5264 ns |          - |          - |
| GenericEnumerableForeach |   10000 |    34,721.0078 ns |       8.0145 ns |       6.6925 ns |          - |       40 B |
|   PlainEnumerableForeach |   10000 |    57,074.9468 ns |   1,023.5033 ns |     907.3090 ns |   114.7461 |   240040 B |
|                  **ListFor** |   **50000** |    **11,568.3783 ns** |      **10.3076 ns** |       **9.6418 ns** |          **-** |          **-** |
|              ListForeach |   50000 |    23,140.0350 ns |      12.8460 ns |      10.7270 ns |          - |          - |
|  ListAsGenericEnumerable |   50000 |   301,499.4210 ns |   1,517.4560 ns |   1,345.1852 ns |          - |       40 B |
|    ListAsPlainEnumerable |   50000 |   414,063.8532 ns |   5,107.6647 ns |   4,527.8117 ns |   573.7305 |  1200040 B |
|                 ArrayFor |   50000 |    11,569.2730 ns |       1.4147 ns |       1.1045 ns |          - |          - |
|             ArrayForeach |   50000 |    11,599.9198 ns |      43.7063 ns |      40.8829 ns |          - |          - |
|        DictionaryForeach |   50000 |   125,040.4834 ns |   1,901.0458 ns |   1,778.2394 ns |          - |          - |
|           HashSetForeach |   50000 |   116,394.2043 ns |     369.0860 ns |     327.1851 ns |          - |          - |
| GenericEnumerableForeach |   50000 |   174,420.6281 ns |   1,132.0933 ns |   1,003.5712 ns |          - |       40 B |
|   PlainEnumerableForeach |   50000 |   283,938.1627 ns |   3,186.8432 ns |   2,825.0535 ns |   573.7305 |  1200040 B |
|                  **ListFor** |  **500000** |   **115,662.8026 ns** |     **114.1001 ns** |     **101.1468 ns** |          **-** |          **-** |
|              ListForeach |  500000 |   234,420.9359 ns |   1,680.7054 ns |   1,572.1328 ns |          - |          - |
|  ListAsGenericEnumerable |  500000 | 3,022,308.2632 ns |  10,492.7929 ns |   8,761.9573 ns |          - |       42 B |
|    ListAsPlainEnumerable |  500000 | 4,434,421.3588 ns |  83,800.3035 ns | 180,388.9939 ns |  5734.3750 | 12000045 B |
|                 ArrayFor |  500000 |   116,606.0909 ns |   1,013.4052 ns |     947.9398 ns |          - |          - |
|             ArrayForeach |  500000 |   115,858.9059 ns |     228.7989 ns |     202.8242 ns |          - |          - |
|        DictionaryForeach |  500000 | 1,325,311.5234 ns |  10,042.4903 ns |   9,393.7517 ns |          - |        1 B |
|           HashSetForeach |  500000 | 1,186,894.8242 ns |   5,663.4456 ns |   5,020.4970 ns |          - |        1 B |
| GenericEnumerableForeach |  500000 | 1,736,346.7611 ns |   2,753.2441 ns |   2,149.5519 ns |          - |       41 B |
|   PlainEnumerableForeach |  500000 | 2,902,051.6927 ns |  27,728.8932 ns |  25,937.6239 ns |  5738.2813 | 12000042 B |
|                  **ListFor** | **1000000** |   **231,202.6768 ns** |      **31.5023 ns** |      **27.9260 ns** |          **-** |          **-** |
|              ListForeach | 1000000 |   478,192.1596 ns |   5,163.1124 ns |   4,576.9647 ns |          - |          - |
|  ListAsGenericEnumerable | 1000000 | 6,066,738.0409 ns |   3,530.0601 ns |   2,947.7601 ns |          - |       45 B |
|    ListAsPlainEnumerable | 1000000 | 8,478,880.5804 ns |  16,042.5931 ns |  14,221.3409 ns | 11468.7500 | 24000049 B |
|                 ArrayFor | 1000000 |   231,292.8298 ns |     121.2219 ns |     101.2258 ns |          - |          - |
|             ArrayForeach | 1000000 |   231,233.6461 ns |      67.5700 ns |      59.8990 ns |          - |          - |
|        DictionaryForeach | 1000000 | 2,591,190.0260 ns |   4,480.0627 ns |   4,190.6534 ns |          - |        2 B |
|           HashSetForeach | 1000000 | 2,477,234.8145 ns |  47,770.9472 ns |  46,917.4576 ns |          - |        2 B |
| GenericEnumerableForeach | 1000000 | 3,479,592.7083 ns |  21,653.4088 ns |  20,254.6120 ns |          - |       42 B |
|   PlainEnumerableForeach | 1000000 | 5,661,172.3125 ns | 105,621.1835 ns | 141,001.2462 ns | 11476.5625 | 24000045 B |

*Observations*:
1. Use ListFor, ArrayFor, ArrayForeach in hotpath scenarios

# Capacity

```csharp
[Benchmark]
public void ListDynamicCapacity()
{
    var list = new List<int>();

    for (int i = 0; i < Records; i++)
    {
        list.Add(i);
    }
}

[Benchmark]
public void ListPlannedCapacity()
{
    var list = new List<int>(Records);

    for (int i = 0; i < Records; i++)
    {
        list.Add(i);
    }
}

[Benchmark]
public void HashSetDynamicCapacity()
{
    var hashSet = new HashSet<int>();

    for (int i = 0; i < Records; i++)
    {
        hashSet.Add(i);
    }
}

[Benchmark]
public void HashSetPlannedCapacity()
{
    var hashSet = new HashSet<int>(Records);

    for (int i = 0; i < Records; i++)
    {
        hashSet.Add(i);
    }
}

[Benchmark]
public void DictionaryDynamicCapacity()
{
    var dictionary = new Dictionary<int, int>();

    for (int i = 0; i < Records; i++)
    {
        dictionary.Add(i, i);
    }
}

[Benchmark]
public void DictionaryPlannedCapacity()
{
    var dictionary = new Dictionary<int, int>(Records);

    for (int i = 0; i < Records; i++)
    {
        dictionary.Add(i, i);
    }
}
```

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|                    Method | Records |             Mean |          Error |         StdDev |           Median |
|-------------------------- |-------- |-----------------:|---------------:|---------------:|-----------------:|
|       **ListDynamicCapacity** |       **5** |         **30.96 ns** |       **0.628 ns** |       **0.838 ns** |         **31.03 ns** |
|       ListPlannedCapacity |       5 |         15.34 ns |       0.335 ns |       0.470 ns |         15.27 ns |
|    HashSetDynamicCapacity |       5 |         78.54 ns |       1.382 ns |       1.292 ns |         78.03 ns |
|    HashSetPlannedCapacity |       5 |         49.08 ns |       1.053 ns |       2.952 ns |         48.35 ns |
| DictionaryDynamicCapacity |       5 |         73.81 ns |       0.367 ns |       0.343 ns |         73.76 ns |
| DictionaryPlannedCapacity |       5 |         55.27 ns |       2.750 ns |       7.846 ns |         51.93 ns |
|       **ListDynamicCapacity** |     **100** |        **274.12 ns** |       **7.734 ns** |      **22.064 ns** |        **265.68 ns** |
|       ListPlannedCapacity |     100 |        219.62 ns |       6.149 ns |      18.131 ns |        218.75 ns |
|    HashSetDynamicCapacity |     100 |      1,192.17 ns |      35.393 ns |      99.826 ns |      1,154.01 ns |
|    HashSetPlannedCapacity |     100 |        591.83 ns |       5.918 ns |       4.621 ns |        590.06 ns |
| DictionaryDynamicCapacity |     100 |      1,259.46 ns |      31.990 ns |      89.704 ns |      1,227.68 ns |
| DictionaryPlannedCapacity |     100 |        646.60 ns |      11.645 ns |      10.893 ns |        644.57 ns |
|       **ListDynamicCapacity** |     **500** |      **1,167.00 ns** |      **22.845 ns** |      **35.567 ns** |      **1,161.52 ns** |
|       ListPlannedCapacity |     500 |        923.26 ns |      17.706 ns |      16.562 ns |        920.20 ns |
|    HashSetDynamicCapacity |     500 |      5,231.46 ns |     101.238 ns |     197.457 ns |      5,186.29 ns |
|    HashSetPlannedCapacity |     500 |      3,156.93 ns |      69.762 ns |     202.393 ns |      3,106.26 ns |
| DictionaryDynamicCapacity |     500 |      5,794.68 ns |     180.205 ns |     505.316 ns |      5,597.09 ns |
| DictionaryPlannedCapacity |     500 |      3,107.39 ns |      22.879 ns |      21.401 ns |      3,107.95 ns |
|       **ListDynamicCapacity** |    **1000** |      **2,023.57 ns** |       **7.969 ns** |       **7.064 ns** |      **2,023.73 ns** |
|       ListPlannedCapacity |    1000 |      1,879.33 ns |      37.459 ns |      73.061 ns |      1,861.80 ns |
|    HashSetDynamicCapacity |    1000 |     10,269.80 ns |      56.354 ns |      52.713 ns |     10,278.31 ns |
|    HashSetPlannedCapacity |    1000 |      5,954.15 ns |     116.847 ns |     155.987 ns |      5,900.20 ns |
| DictionaryDynamicCapacity |    1000 |     10,920.76 ns |      41.595 ns |      32.475 ns |     10,921.04 ns |
| DictionaryPlannedCapacity |    1000 |      6,431.17 ns |     125.467 ns |     317.071 ns |      6,289.15 ns |
|       **ListDynamicCapacity** |   **10000** |     **24,341.29 ns** |     **889.844 ns** |   **2,581.599 ns** |     **23,563.24 ns** |
|       ListPlannedCapacity |   10000 |     17,833.66 ns |     246.274 ns |     192.274 ns |     17,805.74 ns |
|    HashSetDynamicCapacity |   10000 |    194,971.65 ns |     453.202 ns |     378.444 ns |    195,124.29 ns |
|    HashSetPlannedCapacity |   10000 |    105,787.94 ns |   2,051.943 ns |   2,595.051 ns |    104,873.44 ns |
| DictionaryDynamicCapacity |   10000 |    251,574.14 ns |   4,717.904 ns |   8,976.302 ns |    249,212.79 ns |
| DictionaryPlannedCapacity |   10000 |    127,167.95 ns |   2,903.606 ns |   8,284.152 ns |    124,126.07 ns |
|       **ListDynamicCapacity** |   **50000** |    **245,016.99 ns** |   **4,797.808 ns** |   **4,926.995 ns** |    **244,438.84 ns** |
|       ListPlannedCapacity |   50000 |    160,207.69 ns |   4,180.208 ns |  11,721.751 ns |    155,689.55 ns |
|    HashSetDynamicCapacity |   50000 |    857,795.73 ns |  17,139.370 ns |  27,184.857 ns |    854,498.83 ns |
|    HashSetPlannedCapacity |   50000 |    593,178.32 ns |  13,497.508 ns |  38,509.150 ns |    575,435.21 ns |
| DictionaryDynamicCapacity |   50000 |    994,582.30 ns |  18,099.937 ns |  28,179.421 ns |    986,110.06 ns |
| DictionaryPlannedCapacity |   50000 |    535,771.51 ns |   5,502.792 ns |   4,878.082 ns |    535,672.66 ns |
|       **ListDynamicCapacity** |  **500000** |  **2,377,466.29 ns** |  **46,060.148 ns** |  **40,831.121 ns** |  **2,379,448.63 ns** |
|       ListPlannedCapacity |  500000 |  1,551,651.50 ns |  30,296.879 ns |  55,399.557 ns |  1,528,923.34 ns |
|    HashSetDynamicCapacity |  500000 | 10,359,871.66 ns | 205,769.946 ns | 508,612.326 ns | 10,364,231.25 ns |
|    HashSetPlannedCapacity |  500000 |  4,647,332.97 ns |  91,771.325 ns | 223,384.090 ns |  4,650,121.88 ns |
| DictionaryDynamicCapacity |  500000 | 10,682,220.49 ns | 211,737.353 ns | 226,556.596 ns | 10,744,809.38 ns |
| DictionaryPlannedCapacity |  500000 |  4,796,205.89 ns |  95,620.990 ns | 172,424.369 ns |  4,747,389.84 ns |
|       **ListDynamicCapacity** | **1000000** |  **4,943,331.80 ns** |  **96,607.778 ns** |  **99,209.075 ns** |  **4,977,842.97 ns** |
|       ListPlannedCapacity | 1000000 |  3,014,976.09 ns |  52,911.705 ns |  58,811.220 ns |  3,017,630.08 ns |
|    HashSetDynamicCapacity | 1000000 | 17,668,914.06 ns | 345,827.621 ns | 339,648.965 ns | 17,596,417.19 ns |
|    HashSetPlannedCapacity | 1000000 |  8,475,631.97 ns |  97,790.202 ns |  81,659.248 ns |  8,479,632.81 ns |
| DictionaryDynamicCapacity | 1000000 | 20,343,616.67 ns | 404,671.310 ns | 567,292.659 ns | 20,241,881.25 ns |
| DictionaryPlannedCapacity | 1000000 |  9,823,887.11 ns | 175,312.740 ns | 136,872.656 ns |  9,786,100.78 ns |

*Observations*:
1. Always use capacity when you know expected collection size

# Concatenation

**Loop statement**

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|                     Method |       Mean |     Error |    StdDev |    Gen0 | Allocated |
|--------------------------- |-----------:|----------:|----------:|--------:|----------:|
|              StringBuilder |   837.6 ns |  16.78 ns |  47.88 ns |  0.6733 |    1408 B |
|               StringConcat | 2,774.6 ns |  55.47 ns | 106.87 ns | 11.3487 |   23736 B |
|        StringInterpolation | 6,534.7 ns | 170.19 ns | 491.05 ns | 11.4594 |   23976 B |
| StringInterpolationHandler |   681.5 ns |  13.69 ns |  33.05 ns |  0.1945 |     408 B |

**Concatenation of mix value and reference types**

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|                           Method |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|--------------------------------- |----------:|---------:|---------:|-------:|----------:|
|                     StringFormat |  79.34 ns | 1.002 ns | 0.783 ns | 0.0573 |     120 B |
|              StringInterpolation |  54.01 ns | 0.922 ns | 0.906 ns | 0.0459 |      96 B |
|                     StringConcat |  51.08 ns | 0.208 ns | 0.173 ns | 0.0918 |     192 B |
|                       StringJoin |  74.55 ns | 0.593 ns | 0.526 ns | 0.1032 |     216 B |
|                    StringBuilder |  84.85 ns | 0.311 ns | 0.305 ns | 0.2104 |     440 B |
| DefaultInterpolatedStringHandler |  50.56 ns | 0.431 ns | 0.360 ns | 0.0459 |      96 B |
|              EnumerableAggregate | 150.56 ns | 1.761 ns | 1.648 ns | 0.2716 |     568 B |

**Concatenation of strings only**

```

BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.202
  [Host]   : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
|                           Method |      Mean |    Error |   StdDev |    Median |   Gen0 | Allocated |
|--------------------------------- |----------:|---------:|---------:|----------:|-------:|----------:|
|                     StringFormat |  80.89 ns | 1.662 ns | 1.847 ns |  79.89 ns | 0.0459 |      96 B |
|              StringInterpolation |  44.67 ns | 0.319 ns | 0.283 ns |  44.70 ns | 0.0459 |      96 B |
|                       StringJoin |  48.15 ns | 0.168 ns | 0.141 ns |  48.13 ns | 0.0765 |     160 B |
|                    StringBuilder |  80.18 ns | 1.538 ns | 3.656 ns |  78.35 ns | 0.2104 |     440 B |
| DefaultInterpolatedStringHandler |  46.24 ns | 0.931 ns | 1.108 ns |  46.23 ns | 0.0459 |      96 B |
|              EnumerableAggregate | 135.90 ns | 2.160 ns | 1.686 ns | 135.51 ns | 0.2563 |     536 B |

*Observations*:
1. With C# 10 and higher, use string interpolation instead of string.Format, it allocates much less extra memory besides the final string
2. Use DefaultInterpolatedStringHandler or StringBuilder in multiple concatenation statements

**Logging extesnions and LoggerMessage**

``` 
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700HX, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 [AttachedDebugger]
  .NET 8.0 : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0

```

| Method                                | Mean     | Error   | StdDev  | Allocated |
|-------------------------------------- |---------:|--------:|--------:|----------:|
| LogInformation_WithMultipleValueTypes | 326.8 us | 6.45 us | 6.90 us |     955 B |
| LoggerMessage_WithMultipleValueTypes  | 309.9 us | 6.14 us | 8.19 us |     608 B |

*Observations*:
1. Utilise LoggerMessage in hot path scenarios or low-level libraries to avoid unnecessary allocation due to boxing of value types