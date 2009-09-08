﻿/*
 * Copyright (C) 2007, Robin Rosenberg <robin.rosenberg@dewire.com>
 * Copyright (C) 2008, Shawn O. Pearce <spearce@spearce.org>
 * Copyright (C) 2008, Kevin Thompson <kevin.thompson@theautomaters.com>
 * Copyright (C) 2009, Henon <meinrad.recheis@gmail.com>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the Git Development Community nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.IO;
using GitSharp.Transport;

namespace GitSharp
{
    public class PackIndexWriterV2 : PackIndexWriter
    {
        public PackIndexWriterV2(Stream output)
            : base(output)
        {
        }

        internal override void WriteInternal()
        {
            WriteTOC(2);
            WriteFanOutTable();
            WriteObjectNames();
            WriteCRCs();
            WriteOffset32();
            WriteOffset64();
            WriteChecksumFooter();
        }

        private void WriteObjectNames()
        {
            foreach (PackedObjectInfo oe in entries)
            {
            	_stream.Write(oe);
            }
        }

        private void WriteCRCs()
        {
            foreach (PackedObjectInfo oe in entries)
            {
            	_stream.Write(oe.CRC);
            }
        }

        private void WriteOffset32()
        {
            int o64 = 0;
            foreach (PackedObjectInfo oe in entries)
            {
                long o = oe.Offset;
                if (o < int.MaxValue)
                {
                	_stream.Write((int)o);
                }
                else
                {
                	_stream.Write((1 << 31) | o64++);
                }
            }
        }

        private void WriteOffset64()
        {
            foreach (PackedObjectInfo oe in entries)
            {
                long o = oe.Offset;
                if (o > int.MaxValue)
                {
                	_stream.Write(o);
                }
            }
        }
    }
}