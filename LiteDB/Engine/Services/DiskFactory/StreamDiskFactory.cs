﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace LiteDB.Engine
{
    /// <summary>
    /// Simple stream disk implementation of disk factory - used for Memory/Temp database
    /// </summary>
    internal class StreamDiskFactory : IDiskFactory
    {
        private readonly Stream _stream;

        public StreamDiskFactory(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Stream has no name (use stream type)
        /// </summary>
        public string Filename => _stream is MemoryStream ? ":memory:" : _stream is TempStream ? ":temp:" : ":stream:";

        /// <summary>
        /// Use ConcurrentStream wrapper to support multi thread in same Stream (using lock control)
        /// </summary>
        public Stream GetStream(bool canWrite, bool sequencial) => new ConcurrentStream(_stream, canWrite);

        public bool Exists() => _stream.Length > 0;

        public void Delete()
        {
            // stream factory do not delete wal file
        }

        /// <summary>
        /// Do no dispose on finish
        /// </summary>
        public bool CloseOnDispose => false;
    }
}