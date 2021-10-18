# net6-finalizer-repro

Repro for: https://github.com/xamarin/xamarin-android/pull/6363#issuecomment-937379007

This repro's for me with:

    > dotnet --version
    6.0.100-rc.2.21505.57

If you have the `android` workload installed, you should be able to just do:

    > dotnet build -t:Run


