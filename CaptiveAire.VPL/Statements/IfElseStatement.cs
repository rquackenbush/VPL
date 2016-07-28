﻿using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Statements
{
    public class IfElseStatement : CompoundStatementViewModel
    {
        private readonly ParameterViewModel Condition;
        private readonly BlockViewModel IfBlock;
        private readonly BlockViewModel ElseBlock;

        public IfElseStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.IfElse)
        {
            Condition = new ParameterViewModel(owner, "condition", owner.GetVplType(VplTypeId.Boolean));

            IfBlock = new BlockViewModel(owner, "0")
            {
                Label = "If"
            };

            IfBlock.Parameters.Add(Condition);

            ElseBlock = new BlockViewModel(owner, "1")
            {
                Label = "Else"
            };

            Blocks.Add(IfBlock);
            Blocks.Add(ElseBlock);

            //TODO: Add actions for adding / removing else clauses.
            //AddAction(new ElementAction("Add Clause", () => MessageBox.Show("Not Implemented"), () => true));
        }

        protected override async Task ExecuteCoreAsync(CancellationToken token)
        {
            var condition = (bool)await Condition.EvaluateAsync(token);

            if (condition)
            {
                await IfBlock.ExecuteAsync(token);
            }
            else
            {
                await ElseBlock.ExecuteAsync(token);
            }
        }
    }
}