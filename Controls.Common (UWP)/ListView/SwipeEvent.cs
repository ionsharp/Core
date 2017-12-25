#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Animation;

namespace Imagin.Controls.Common
{
    public delegate void SwipeBeginEventHandler(object sender);

    public delegate void SwipeProgressEventHandler(object sender, SwipeProgressEventArgs args);

    public delegate void SwipeCompleteEventHandler(object sender, SwipeCompleteEventArgs args);

    public delegate void SwipeReleaseEventHandler(object sender, SwipeReleaseEventArgs args);

    public delegate void SwipeTriggerEventHandler(object sender, SwipeTriggerEventArgs args);

    public class SwipeProgressEventArgs
    {
        public SwipeProgressEventArgs(SwipeDirection direction, double cumulative, double delta, double currRate)
        {
            SwipeDirection = direction;
            Cumulative = cumulative;
            CurrentRate = currRate;
            Delta = delta;
        }

        public SwipeDirection SwipeDirection { get; set; }

        public double Cumulative { get; set; }

        public double Delta { get; set; }

        public double CurrentRate { get; set; }
    }

    public class SwipeCompleteEventArgs
    {
        public SwipeCompleteEventArgs(SwipeDirection direction)
        {
            SwipeDirection = direction;
        }

        public SwipeDirection SwipeDirection { get; set; }
    }

    public class SwipeReleaseEventArgs
    {
        public SwipeReleaseEventArgs(SwipeDirection direction, EasingFunctionBase easingFunc, double itemToX, double duration)
        {
            SwipeDirection = direction;
            EasingFunc = easingFunc;
            ItemToX = itemToX;
            Duration = duration;
        }

        public SwipeDirection SwipeDirection { get; set; }

        public EasingFunctionBase EasingFunc { get; set; }

        public double ItemToX { get; set; }

        public double Duration { get; set; }
    }

    public class SwipeTriggerEventArgs
    {
        public SwipeTriggerEventArgs(SwipeDirection direction, bool isTrigger)
        {
            SwipeDirection = direction;
            IsTrigger = isTrigger;
        }

        public SwipeDirection SwipeDirection { get; set; }

        public bool IsTrigger { get; set; }
    }
}
