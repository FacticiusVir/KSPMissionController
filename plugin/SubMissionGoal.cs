using System;
using System.Collections.Generic;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// A submission that consists of several mission goals
    /// </summary>
    public class SubMissionGoal : MissionGoal
    {
        public List<MissionGoal> subGoals = new List<MissionGoal>();

        public SubMissionGoal() {
            this.nonPermanent = true;
        }

        public void add(MissionGoal c) {
            subGoals.Add (c);
        }

        public override bool isDone (Vessel vessel, GameEvent events)
        {
            bool value = base.isDone (vessel, events);
            if (value && nonPermanent) {
                foreach (MissionGoal g in subGoals) {
                    g.doneOnce = true;
                }
            }
            return value;
        }

        protected override List<Value> values(Vessel vessel, GameEvent events) {
            List<Value> values = new List<Value> ();

            foreach (MissionGoal c in subGoals) {
                values = values.Union(c.getValues(vessel, events)).ToList();
            }

            return values;
        }

        public override String getType() {
            return "Submission";
        }
    }
}

