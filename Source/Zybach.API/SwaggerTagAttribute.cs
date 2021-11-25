using System;

namespace Zybach.API
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SwaggerTagAttribute : Attribute
    {
    }
}