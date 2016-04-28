﻿using SkyCrab.Common_classes;
using SkyCrab.Connection.PresentationLayer.DataTranscoders.NativeTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyCrab.Connection.PresentationLayer.DataTranscoders.SkyCrabTypes
{
    internal sealed class LimitedStringTranscoder : ITranscoder<String>
    {

        private static readonly Dictionary<LengthLimit, LimitedStringTranscoder> instances = new Dictionary<LengthLimit, LimitedStringTranscoder>();
        public static LimitedStringTranscoder Get(LengthLimit limit)
        {
            LimitedStringTranscoder instance;
            if (instances.TryGetValue(limit, out instance))
                return instance;
            instance = new LimitedStringTranscoder(limit);
            instances.Add(limit, instance);
            return instance;
        }

        private LengthLimit limit;


        private LimitedStringTranscoder(LengthLimit limit)
        {
            this.limit = limit;
        }

        public String Read(EncryptedConnection connection)
        {
            bool isNull = BoolTranscoder.Get.Read(connection);
            if (isNull)
                return null;
            UInt16 length = UInt16Transcoder.Get.Read(connection);
            if (length == 0)
            {
                if (limit.Min == 0)
                    return "";
                else
                    return null;
            }
            limit.checkAndThrow(length);
            byte[] bytes = connection.SyncReadBytes(length);
            string data = Encoding.UTF8.GetString(bytes);
            return data;
        }

        public void Write(EncryptedConnection connection, object writingBlock, String data)
        {
            BoolTranscoder.Get.Write(connection, writingBlock, data == null);
            if (data == null)
                return;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            UInt16 lenght = (UInt16)bytes.Length;
            limit.checkAndThrow(lenght);
            UInt16Transcoder.Get.Write(connection, writingBlock, lenght);
            if (lenght != 0)
                connection.AsyncWriteBytes(writingBlock, bytes);
        }

    }
}
