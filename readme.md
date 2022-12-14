This projects aims to just check if there is any point in even trying to add optimistic in-place mutation for C# records.

That means that if original record is never used again, when doing `record = record with { Property = value }` it should be possible to just mutate the original record in-place and save time on allocations.

Results on my machine:


|                             Method | ItemCount |              Mean |             Error |            StdDev |      Gen0 |      Gen1 |     Gen2 |  Allocated |
|----------------------------------- |---------- |------------------:|------------------:|------------------:|----------:|----------:|---------:|-----------:|
|                CopyMultipleRecords |      1000 |     34,109.856 ns |       484.5953 ns |       453.2907 ns |   49.6216 |         - |        - |   103936 B |
|                      ModifyClasses |      1000 |     27,107.895 ns |       352.9886 ns |       330.1857 ns |   34.1797 |         - |        - |    71936 B |
| MultipleModificationsInARow_Record |      1000 |         15.024 ns |         0.3296 ns |         0.3664 ns |    0.0459 |         - |        - |       96 B |
|  MultipleModificationsInARow_Class |      1000 |          5.666 ns |         0.1113 ns |         0.1041 ns |    0.0153 |         - |        - |       32 B |
|                CopyMultipleRecords |     10000 |    573,340.067 ns |     7,791.3767 ns |     6,906.8525 ns |  211.9141 |  143.5547 |        - |  1039936 B |
|                      ModifyClasses |     10000 |    425,164.225 ns |     8,271.3656 ns |    19,817.6636 ns |  158.2031 |   90.8203 |        - |   719936 B |
| MultipleModificationsInARow_Record |     10000 |         18.464 ns |         0.3847 ns |         0.4430 ns |    0.0459 |         - |        - |       96 B |
|  MultipleModificationsInARow_Class |     10000 |          6.512 ns |         0.1558 ns |         0.1913 ns |    0.0153 |         - |        - |       32 B |
|                CopyMultipleRecords |    100000 | 22,007,577.557 ns | 1,118,663.1573 ns | 3,280,845.3923 ns | 1531.2500 | 1031.2500 | 500.0000 | 10400119 B |
|                      ModifyClasses |    100000 | 11,293,347.922 ns |   680,531.0006 ns | 2,006,561.0095 ns | 1125.0000 |  750.0000 | 328.1250 |  7200058 B |
| MultipleModificationsInARow_Record |    100000 |         16.383 ns |         0.4765 ns |         1.3284 ns |    0.0459 |         - |        - |       96 B |
|  MultipleModificationsInARow_Class |    100000 |          6.737 ns |         0.2017 ns |         0.5852 ns |    0.0153 |         - |        - |       32 B |


it seems like mutation is indeed faster.

