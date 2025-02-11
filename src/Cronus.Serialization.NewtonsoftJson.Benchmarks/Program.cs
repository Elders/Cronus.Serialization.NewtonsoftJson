using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var cfg = DefaultConfig.Instance.StopOnFirstError();
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, cfg);
