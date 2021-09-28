using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace SlapCityVoiceMod.Classes
{
    [System.Serializable]
    public class MovesetPatch
    {
        public string path;
        public string hash;

        public JsonPatchDocument patch;

        public bool TryLoadMovesetPatch(string json)
        {
            try
            {
                var operationsToValidate = JsonConvert.DeserializeObject<List<Patch>>(json);

                foreach (var operation in operationsToValidate)
                {
                    if (operation.value.t == null || operation.value.t != "SAPVL")
                    {
                        Plugin.LogError($"Invalid action type: {operation.value.t}");
                        return false;
                    }
                }

                patch = new JsonPatchDocument(JsonConvert.DeserializeObject<List<Operation>>(json), new DefaultContractResolver());
            }
            catch (Exception e)
            {
                Plugin.LogError($"Error parsing moveset patch!\n{e.Message}\n{e.StackTrace}");
                return false;
            }

            return true;
        }

        public class Patch
        {
            public string op;
            public string path;
            public PatchValue value;
        }

        public class PatchValue
        {
            public string t;
            public string s;
        }
    }
}
