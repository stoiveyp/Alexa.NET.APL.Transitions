using System;
using System.Collections.Generic;
using Alexa.NET.APL.Commands;
using Alexa.NET.Response.APL;

namespace Alexa.NET.APL
{
    public static class Transitions
    {
        public static CommandDefinition RollIn() => new CommandDefinition
        {
            Parameters = new List<Parameter>
            {
                new Parameter("duration"),
                new Parameter("delay")
            },
            Commands = new List<APLCommand>
            {
                new AnimateItem
                {
                    Duration = APLValue.To<int?>("${duration}"),
                    DelayMilliseconds = APLValue.To<int?>("${delay || 0}"),
                    Value = new List<AnimatedProperty>
                    {
                        new AnimatedOpacity
                        {
                            From=0.0,
                            To =1.0
                        },
                        new AnimatedTransform{
                            From = new List<APLTransform>
                            {
                                new APLTransform
                                {
                                    TranslateX = APLValue.To<double?>("-100%")
                                },
                                new APLTransform
                                {
                                    Rotate = -120.0
                                }
                            },
                            To = new List<APLTransform>
                            {
                                new APLTransform
                                {
                                    TranslateX = APLValue.To<double?>("0")
                                },
                                new APLTransform
                                {
                                    Rotate = 0.0
                                }
                            },
                        }
                    }
                }
            }
        };
    }
}

