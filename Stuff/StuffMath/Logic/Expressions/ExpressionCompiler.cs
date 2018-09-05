using Stuff.StuffMath.Logic.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions
{
    public class ExpressionCompiler
    {
        public enum TokenType { Value, Identifier, Operator, SBracket, EBracket, Negation};

        public static readonly string[] Operators = { "+", "*", ">", "=", "|", "&"};

        public class Token
        {
            public TokenType Type { get; private set; }

            public string Value { get; private set; }

            public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }

            public override string ToString()
            {
                return "{" + Type + ": " + Value + "}";
            }
        }

        public static LinkedList<Token> Tokenize(string exp)
        {
            exp = new string(exp.Where(c => c != ' ' && c != ' ').ToArray());

            var result = new LinkedList<Token>();

            string curIdentifier = "";

            int brackets = 0;

            int pos = 0;

            foreach (char c in exp)
            {
                if (curIdentifier.Length > 0)
                {
                    if (!char.IsLetter(c) && !char.IsNumber(c))
                    {
                        result.AddLast(new Token(TokenType.Identifier, curIdentifier));
                        curIdentifier = "";
                    }
                    else
                        curIdentifier += c;
                }
                if (c == '(')
                {
                    if (pos != 0 && result.Last().Type == TokenType.Value)
                        throw new Exception("Literals cannot be followed by an opening bracket. Implicit multiplication is not possible. Position: " + pos + " String: " + exp);
                    if (pos != 0 && result.Last().Type == TokenType.Identifier)
                        throw new Exception("Identifiers cannot be followed by an opening bracket. Position: " + pos + " String: " + exp);
                    result.AddLast(new Token(TokenType.SBracket, "" + c));
                    brackets++;
                }
                else if (c == ')')
                {
                    if (pos == 0)
                        throw new Exception("Expressions cannot start with a closing bracket. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.Operator)
                        throw new Exception("Operators cannot be followed by a closing bracket; they must have an operand on both sides. Position: " + pos + " String: " + exp);

                    result.AddLast(new Token(TokenType.EBracket, "" + c));
                    brackets--;
                    if (brackets < 0)
                        throw new Exception("Closing bracket at " + pos + " has no matching opening bracket. String: " + exp);
                }
                else if (char.IsLetter(c))
                {
                    if (curIdentifier.Length == 0)
                    {
                        if (pos != 0 && result.Last().Type == TokenType.EBracket)
                            throw new Exception("Closing brackets cannot be followed by an identifier; they must be followed by an operator. Position: " + pos + " String: " + exp);
                        curIdentifier += c;
                    }
                }
                else if (c == '1' || c == '0')
                {
                    Debug.Assert(curIdentifier.Length == 0, "The case of curIdentifier being active should have been handled elsewhere. Position: " + pos + " String: " + exp);
                    if (pos != 0 && result.Last().Type == TokenType.EBracket)
                        throw new Exception("Closing brackets cannot be followed by a literal; they must be followed by an operator. Position: " + pos + " String: " + exp);
                    result.AddLast(new Token(TokenType.Value, "" + c));
                }
                else if (c == '!')
                {
                    Debug.Assert(curIdentifier.Length == 0, "The case of curIdentifier being active should have been handled elsewhere. Position: " + pos + " String: " + exp);
                    result.AddLast(new Token(TokenType.Negation, ""));
                }
                else if (Operators.Contains("" + c))
                {
                    if (pos == 0)
                        throw new Exception("Expressions cannot start with an operator. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.SBracket)
                        throw new Exception("Operators cannot be preceded by an opening bracket; they must have an operand on both sides. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.Operator)
                        throw new Exception("Operators cannot follow each other consecutively; they must have an operand on both sides. Position: " + pos + " String: " + exp);
                    result.AddLast(new Token(TokenType.Operator, "" + c));
                }
                else
                    throw new Exception("Invalid symbol '" + c + "'. Position: " + pos + " String: " + exp);
                pos++;
            }
            if (curIdentifier.Length > 0)
            {
                result.AddLast(new Token(TokenType.Identifier, curIdentifier));
                curIdentifier = "";
            }

            if (brackets != 0)
            {
                Debug.Assert(brackets > 0);
                throw new Exception("Not all brackets are closed. String: " + exp);
            }
            return result;
        }

        public static Expression Compile(LinkedList<Token> tokens)
        {
            return InternalCompile(tokens.First, tokens.Last);
        }

        private static Expression InternalCompile(LinkedListNode<Token> first, LinkedListNode<Token> last, int level = 0)
        {
            if (first.Value.Type == TokenType.SBracket && MatchingBracketForward(first) == last)
                return InternalCompile(first.Next, last.Previous, 0);
            else if (first.Value.Type == TokenType.Negation && first.Next.Value.Type == TokenType.SBracket && MatchingBracketForward(first.Next) == last)
                return new Not(InternalCompile(first.Next.Next, last.Previous, 0));
            LinkedListNode<Token> token;
            int brackets = 0;
            switch (level)
            {
                case 0:
                    Debug.Assert(level == 0);
                    // Equality
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "=")
                            {
                                if (token.Previous.Value.Type == TokenType.Negation)
                                    return new XOr(InternalCompile(first, token.Previous.Previous, level), InternalCompile(token.Next, last, level));
                                else
                                    return new Iff(InternalCompile(first, token.Previous, level), InternalCompile(token.Next, last, level));
                            }
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    Debug.Assert(brackets == 0, "Unbalanced brackets");
                    level++;
                    goto case 1;
                case 1:
                    Debug.Assert(level == 1);
                    // Implication
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == ">")
                            {
                                if (token.Previous.Value.Type == TokenType.Negation)
                                    throw new Exception("Implication operator cannot be negated. Tokens: " + ContainerUtils.AsString(first, last));
                                return new Implies(InternalCompile(first, token.Previous, level), InternalCompile(token.Next, last, level));
                            }
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    if (brackets != 0)
                        throw new Exception("Unbalanced brackets");
                    level++;
                    goto case 2;
                case 2:
                    Debug.Assert(level == 2);
                    // Or
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "+" || token.Value.Value == "|")
                            {
                                if (token.Previous.Value.Type == TokenType.Negation)
                                    return new NOr(InternalCompile(first, token.Previous.Previous, level), InternalCompile(token.Next, last, level));
                                else
                                    return new Or(InternalCompile(first, token.Previous, level), InternalCompile(token.Next, last, level));
                            }
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    if (brackets != 0)
                        throw new Exception("Unbalanced brackets");
                    level++;
                    goto case 3;
                case 3:
                    Debug.Assert(level == 3);
                    // And
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "*" || token.Value.Value == "&")
                            {
                                if (token.Previous.Value.Type == TokenType.Negation)
                                    return new NAnd(InternalCompile(first, token.Previous.Previous, level), InternalCompile(token.Next, last, level));
                                else
                                    return new And(InternalCompile(first, token.Previous, level), InternalCompile(token.Next, last, level));
                            }
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    if (brackets != 0)
                        throw new Exception("Unbalanced brackets");
                    level++;
                    goto case 4;
                case 4:
                    Debug.Assert(level == 4);
                    // Variables
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Type == TokenType.Identifier)
                            {
                                if (token != first && token.Previous.Value.Type == TokenType.Negation)
                                    return new Not(new Variable(token.Value.Value));
                                else
                                    return new Variable(token.Value.Value);
                            }
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    if (brackets != 0)
                        throw new Exception("Unbalanced brackets");
                    level++;
                    goto case 5;
                case 5:
                    Debug.Assert(level == 5);
                    // Literals
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Type == TokenType.Value)
                                return new ValueExpression(token.Value.Value == "1");
                        }
                        else if (brackets <= 0)
                            throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
                        token = token.Previous;
                    }
                    if (brackets != 0)
                        throw new Exception("Unbalanced brackets");
                    //level++;
                    goto default;
                default:
                    throw new Exception("SumTingWong. Tokens: " + ContainerUtils.AsString(first, last));
            }
        }

        private static LinkedListNode<Token> MatchingBracketForward(LinkedListNode<Token> sBracket)
        {
            Debug.Assert(sBracket.Value.Type == TokenType.SBracket);
            int brackets = 1;
            while (sBracket != null)
            {
                sBracket = sBracket.Next;
                if (sBracket.Value.Type == TokenType.SBracket)
                    brackets++;
                else if (sBracket.Value.Type == TokenType.EBracket)
                    brackets--;
                if (brackets == 0)
                    return sBracket;
            }
            throw new Exception("No matching bracket found.");
        }

        private static LinkedListNode<Token> MatchingBracketBackward(LinkedListNode<Token> eBracket)
        {
            Debug.Assert(eBracket.Value.Type == TokenType.EBracket);
            int brackets = 1;
            while (eBracket != null)
            {
                eBracket = eBracket.Previous;
                if (eBracket.Value.Type == TokenType.EBracket)
                    brackets++;
                else if (eBracket.Value.Type == TokenType.SBracket)
                    brackets--;
                if (brackets == 0)
                    return eBracket;
            }
            throw new Exception("No matching bracket found.");
        }
    }
}
