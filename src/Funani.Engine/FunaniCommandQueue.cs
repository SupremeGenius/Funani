﻿/*
 * Copyright (c) 2012-2016, Jaap de Haan <jaap.dehaan@color-of-code.de>
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *   * Redistributions of source code must retain the above copyright notice,
 *     this list of conditions and the following disclaimer.
 *   * Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 *   * Neither the name of the "Color-Of-Code" nor the names of its
 *     contributors may be used to endorse or promote products derived from
 *     this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using Funani.Api;

namespace Funani.Engine
{
    public class FunaniCommandQueue : ICommandQueue
    {
        private readonly Queue<ICommand> _actions;
        private Int32 _processed;
        private Thread _thread;

        public FunaniCommandQueue()
        {
            _thread = null;
            _actions = new Queue<ICommand>();
        }

        public void AddCommand(ICommand action)
        {
            lock (_actions)
            {
                _actions.Enqueue(action);
                if (_thread == null)
                {
                    _thread = new Thread(Worker);
                    _thread.Start();
                }
            }
        }

        public Int32 Count
        {
            get
            {
                lock (_actions)
                {
                    return _actions.Count + _processed;
                }
            }
        }

        public event EventHandler ThreadStarted;
        public event EventHandler ThreadEnded;
        public event EventHandler<CommandProgressEventArgs> CommandStarted;
        public event EventHandler<CommandProgressEventArgs> CommandEnded;

        private void Worker(object parameter)
        {
            try
            {
                Setup();

                while (_actions.Count > 0)
                {
                    ICommand command = _actions.Peek();
                    EventHandler<CommandProgressEventArgs> handler = CommandStarted;
                    if (handler != null)
                        handler(this, new CommandProgressEventArgs(command));

                    command.Execute(null);

                    lock (_actions)
                    {
                        _processed++;
                        _actions.Dequeue();
                    }

                    handler = CommandEnded;
                    if (handler != null)
                        handler(this, new CommandProgressEventArgs(command));
                }
            }
            finally
            {
                TearDown();
            }
        }

        private void TearDown()
        {
            _thread = null;
            EventHandler handler = ThreadEnded;
            if (handler != null)
                handler(this, null);
        }

        private void Setup()
        {
            _processed = 0;
            EventHandler handler = ThreadStarted;
            if (handler != null)
                handler(this, null);
        }
    }
}