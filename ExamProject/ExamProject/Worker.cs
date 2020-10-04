using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ExamProject {
    class Worker<T>  {
        public Worker(string fileName) {
            if (!File.Exists(fileName)) {
                File.Create(fileName);
            }
            Data = new List<T>();
            this.fileName = fileName;

        }
        public  List<T> Data { get; private set; }

        private readonly string fileName;
        public void Load() {
            try {
                string text = File.ReadAllText(fileName);
                var deserializedData = JsonSerializer.Deserialize<List<T>>(text);
                Data = deserializedData;
            } catch {}

        }
        public void Save() {
            var serializedData = JsonSerializer.Serialize(Data);
            File.WriteAllText(fileName, serializedData);
        }


    }
}
