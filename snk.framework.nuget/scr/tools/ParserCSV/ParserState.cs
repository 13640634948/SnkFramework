namespace SnkFramework.NuGet.Tools
{
    public abstract class ParserState
    {
        protected ParserStateMachine machine;
        protected ParserState(ParserStateMachine machine)
        {
            this.machine = machine;
        }

        public virtual void AnyChar(char ch)
        {
            throw new System.NotImplementedException("ParserState.AnyChar");
        }

        public virtual void Comma()
        {
            throw new System.NotImplementedException("ParserState.Comma");
        }

        public virtual void Quote()
        {
            throw new System.NotImplementedException("ParserState.Quote");
        }

        public virtual void EndOfLine()
        {
            throw new System.NotImplementedException("ParserState.EndOfLine");
        }
    }
}
