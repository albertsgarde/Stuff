using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Stuff;

namespace Stuff.StuffMath.Expressions
{
    public static class ExpressionCompiler
    {
        public enum TokenType { Value, Identifier, Operator, SBracket, EBracket, Comma };

        public static readonly string[] Operators = { "+", "-", "*", "/", "^" };

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

            string curNum = "";

            string curIdentifier = "";

            bool hasDecimal = false;

            int brackets = 0;

            int pos = 0;

            foreach (char c in exp)
            {
                if (curIdentifier.Length > 0)
                {
                    Debug.Assert(curNum.Length == 0, "curNum and curIdentifier should never be active simultaneously. Position: " + pos + " String: " + exp);
                    if (!char.IsLetter(c) && !char.IsNumber(c))
                    {
                        result.AddLast(new Token(TokenType.Identifier, curIdentifier));
                        curIdentifier = "";
                    }
                    else
                        curIdentifier += c;
                }
                else if (curNum.Length > 0)
                {
                    if (char.IsNumber(c))
                        curNum += c;
                    else if (c == '.')
                    {
                        if (hasDecimal)
                            throw new Exception("A number can only contain 1 decimal point.");
                        else
                        {
                            curNum += '.';
                            hasDecimal = true;
                        }
                    }
                    else
                    {
                        if (char.IsLetter(c))
                            throw new Exception("An identifier cannot start with a number, and a number may not be followed by an identifier. Position: " + pos + " String: " + exp);
                        else if (c == '(')
                            throw new Exception("A number cannot be followed by an opening bracket. Implicit multiplication is not possible. Position: " + pos + " String: " + exp);
                        result.AddLast(new Token(TokenType.Value, curNum));
                        curNum = "";
                        hasDecimal = false;
                    }
                }
                if (c == '(')
                {
                    if (pos != 0 && result.Last().Type == TokenType.Value)
                        throw new Exception("Literals cannot be followed by an opening bracket. Implicit multiplication is not possible. Position: " + pos + " String: " + exp);
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
                else if (c == ',')
                {
                    if (brackets == 0)
                        throw new Exception("Commas can only exist inside brackets. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.Operator)
                        throw new Exception("Expected expression, not comma. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.SBracket)
                        throw new Exception("Commas cannot follow a start bracket. only after an expression. Position: " + pos + " String: " + exp);
                    if (result.Last().Type == TokenType.Comma)
                        throw new Exception("Commas cannot follow other commas. Position: " + pos + " String: " + exp);

                    result.AddLast(new Token(TokenType.Comma, "" + c));
                }
                else if (char.IsLetter(c))
                {
                    if (curIdentifier.Length == 0)
                    {
                        Debug.Assert(curNum.Length == 0, "The case of curNum being active should have been handled elsewhere. Position: " + pos + " String: " + exp);
                        if (pos != 0 && result.Last().Type == TokenType.EBracket)
                            throw new Exception("Closing brackets cannot be followed by an identifier; they must be followed by an operator. Position: " + pos + " String: " + exp);
                        curIdentifier += c;
                    }
                }
                else if (char.IsNumber(c) || c == '.')
                {
                    if (curNum.Length == 0 && curIdentifier.Length == 0)
                    {
                        Debug.Assert(curIdentifier.Length == 0, "The case of curIdentifier being active should have been handled elsewhere. Position: " + pos + " String: " + exp);
                        if (pos != 0 && result.Last().Type == TokenType.EBracket)
                            throw new Exception("Closing brackets cannot be followed by a literal; they must be followed by an operator. Position: " + pos + " String: " + exp);
                        curNum += c;
                        if (c == '.')
                            hasDecimal = true;
                    }
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
                Debug.Assert(curNum.Length == 0, "curNum and curIdentifier should never be active simultaneously. Position: " + pos + " String: " + exp);
                result.AddLast(new Token(TokenType.Identifier, curIdentifier));
                curIdentifier = "";
            }
            else if (curNum.Length > 0)
            {
                result.AddLast(new Token(TokenType.Value, curNum));
                curNum = "";
            }

            if (brackets != 0)
            {
                Debug.Assert(brackets > 0);
                throw new Exception("Not all brackets are closed. String: " + exp);
            }
            return result;
        }

        public static Expression Compile(LinkedList<Token> tokens, FunctionManager functions)
        {
            return InternalCompile(tokens.First, tokens.Last, functions);
        }

        private static Expression InternalCompile(LinkedListNode<Token> first, LinkedListNode<Token> last, FunctionManager functions, int level = 0)
        {
            if (first.Value.Type == TokenType.SBracket && MatchingBracketForward(first) == last)
                return InternalCompile(first.Next, last.Previous, functions, 0);
            LinkedListNode<Token> token;
            int brackets = 0;
            switch (level)
            {
                case 0:
                    Debug.Assert(level == 0);
                    // Additions and subtractions
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "+")
                                return functions.Operator("+").Create(InternalCompile(first, token.Previous, functions, level), InternalCompile(token.Next, last, functions, level));
                            if (token.Value.Value == "-")
                                return functions.Operator("-").Create(InternalCompile(first, token.Previous, functions, level), InternalCompile(token.Next, last, functions, level));
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
                    // Multiplications and divisions
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "*")
                                return functions.Operator("*").Create(InternalCompile(first, token.Previous, functions, level), InternalCompile(token.Next, last, functions, level));
                            if (token.Value.Value == "/")
                                return functions.Operator("/").Create(InternalCompile(first, token.Previous, functions, level), InternalCompile(token.Next, last, functions, level));
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
                    // Powers
                    token = last;
                    while (token != first.Previous)
                    {
                        if (token.Value.Type == TokenType.SBracket)
                            brackets--;
                        else if (token.Value.Type == TokenType.EBracket)
                            brackets++;
                        else if (brackets == 0)
                        {
                            if (token.Value.Value == "^")
                                return functions.Operator("^").Create(InternalCompile(first, token.Previous, functions, level), InternalCompile(token.Next, last, functions, level));
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
                    // Functions & variables
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
                                if (token.Next != null && token.Next.Value.Type == TokenType.SBracket)
                                    return functions.Function(token.Value.Value).Create(FunctionArgumentsCompile(token.Next.Next, MatchingBracketForward(token.Next), functions));
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
                    goto case 4;
                case 4:
                    Debug.Assert(level == 4);
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
                                return new ValueExpression(double.Parse(token.Value.Value));
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
                    throw new Exception();
            }
        }

        private static Expression[] FunctionArgumentsCompile(LinkedListNode<Token> first, LinkedListNode<Token> last, FunctionManager functions)
        {
            LinkedListNode<Token> prevComma = first; // Actually just after the previous comma.
            LinkedListNode<Token> token = first;
            List<Expression> exps = new List<Expression>();
            int brackets = 0;
            while (token != last)
            {
                if (token.Value.Type == TokenType.SBracket)
                    brackets--;
                else if (token.Value.Type == TokenType.EBracket)
                    brackets++;
                else if (brackets == 0 && token.Value.Type == TokenType.Comma)
                {
                    exps.Add(InternalCompile(prevComma, token.Previous, functions));
                    prevComma = token.Next;
                }
                token = token.Next;
            }
            Debug.Assert(brackets == 0);
            exps.Add(InternalCompile(prevComma, last.Previous, functions));
            return exps.ToArray();
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
