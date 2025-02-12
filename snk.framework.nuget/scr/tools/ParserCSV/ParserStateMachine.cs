﻿namespace SnkFramework.NuGet.Tools
{
    public class ParserStateMachine
    {
        public LineStartState LineStartState;
        public ValueStartState ValueStartState;
        public ValueState ValueState;
        public QuotedValueState QuotedValueState;
        public QuoteState QuoteState;

        private ParserState currState;
        public ParserContext context;
        public const char CommaCharacter = ',';
        public const char QuoteCharacter = '"';
        public bool TrimTrailingEmptyLines { get; set; }
        public int MaxColumnsToRead { get; set; }
        public ParserStateMachine()
        {
            context = new ParserContext();
            LineStartState = new LineStartState(this);
            ValueStartState = new ValueStartState(this);
            ValueState = new ValueState(this);
            QuotedValueState = new QuotedValueState(this);
            QuoteState = new QuoteState(this);
        }

        public void SetState(ParserState currState)
        {
            this.currState = currState;
        }

        public void AnyChar(char ch)
        {
            currState.AnyChar(ch);
        }

        public void Comma()
        {
            currState.Comma();
        }

        public void EndOfLine()
        {
            currState.EndOfLine();
        }

        public void Quote()
        {
            currState.Quote();
        }
    }

}
