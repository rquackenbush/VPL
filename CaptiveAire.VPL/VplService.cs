﻿using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL
{
    public class VplService : IVplService
    {
        private readonly IVplServiceContext _context;

        public VplService(IEnumerable<IVplPlugin> plugins = null)
        {
            _context = new VplServiceContext(plugins);
        }

        public void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction)
        {
            //Create the view model
            var functionViewModel = CreateRuntimeFunctionInner(metadata);
          
            //Create the view model
            var editorViewModel = new FunctionEditorViewModel(_context, functionViewModel, saveAction);

            //Show the dialog
            _context.ViewService.ShowDialog(editorViewModel);
        }

        public IFunction CreateRuntimeFunction(FunctionMetadata metadata)
        {
            return CreateRuntimeFunctionInner(metadata);
        }

        public IEnumerable<IVplType> Types
        {
            get { return _context.Types; }
        }

        private FunctionViewModel CreateRuntimeFunctionInner(FunctionMetadata metadata)
        {
            //Create the view model
            var functionViewModel = new FunctionViewModel(_context, metadata.Id)
            {
                Name = metadata.Name
            };

            //Create the builder
            var builder = new ElementBuilder(_context.ElementFactoryManager, functionViewModel);

            //Add the elements to the owner (function)
            builder.LoadFunction(functionViewModel, metadata);

            return functionViewModel;
        }
    }
}