﻿namespace SnkFramework.NuGet.Tools
{
    public class ValueStartState : ParserState
    {
        public ValueStartState(ParserStateMachine machine) : base(machine)
        {
        }

        public override void EndOfLine()
        {
            machine.context.AddValue();
            machine.context.AddLine();
            machine.SetState(machine.LineStartState);
        }

        public override void AnyChar(char ch)
        {
            machine.context.AddChar(ch);
            machine.SetState(machine.ValueState);
        }

        public override void Comma()
        {
            machine.context.AddValue();
            machine.SetState(machine.ValueStartState);
        }
        public override void Quote()
        {
            machine.SetState(machine.QuotedValueState);
        }
    }
}