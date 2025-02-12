﻿namespace SnkFramework.NuGet.Tools
{
    public class QuotedValueState : ParserState
    {
        public QuotedValueState(ParserStateMachine machine) : base(machine)
        {
        }

        public override void AnyChar(char ch)
        {
            machine.context.AddChar(ch);
            machine.SetState(machine.QuotedValueState);
        }

        public override void Comma()
        {
            machine.context.AddChar(ParserStateMachine.CommaCharacter);
            machine.SetState(machine.QuotedValueState);
        }

        public override void EndOfLine()
        {
            machine.context.AddChar('\r');
            machine.context.AddChar('\n');
            machine.SetState(machine.QuotedValueState);
        }

        public override void Quote()
        {
            machine.SetState(machine.QuoteState);
        }
    }
}