﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFUtils.Tests.TestClasses
{
    public class TestMessageHandler2 : IFunction<TestMessage>
    {
        public Task InvokeAsync(TestMessage input, ITraceWriter Log)
        {
            InvocationCounter.Instance.AddOne();

            return Task.CompletedTask;
        }
    }
}
