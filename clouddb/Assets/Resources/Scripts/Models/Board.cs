/*
 * Copyright 2022. Huawei Technologies Co., Ltd. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using Huawei.Agconnect.CloudDB;


namespace Assets.Scripts.Models
{
    public class Board : CloudDBZoneObject
    {
        public Board() : base(typeof(Board))
        {
            Gap0 = "";
            Gap1 = "";
            Gap2 = "";
            Gap3 = "";
            Gap4 = "";
            Gap5 = "";
            Gap6 = "";
            Gap7 = "";
            Gap8 = "";
            HasPlayerX = false;
            HasPlayerO = false;
            Turn = "X";
            LastIndex = 0;
        }

        public string Id { get; set; }
        public string Gap0 { get; set; }
        public string Gap1 { get; set; }
        public string Gap2 { get; set; }
        public string Gap3 { get; set; }
        public string Gap4 { get; set; }
        public string Gap5 { get; set; }
        public string Gap6 { get; set; }
        public string Gap7 { get; set; }
        public string Gap8 { get; set; }
        public bool HasPlayerX { get; set; }
        public bool HasPlayerO { get; set; }
        public string Turn { get; set; }
        public int LastIndex { get; set; }
    }
}
