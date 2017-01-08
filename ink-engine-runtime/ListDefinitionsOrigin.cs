﻿using System.Collections.Generic;

namespace Ink.Runtime
{
    internal class ListDefinitionsOrigin
    {
        public ListDefinitionsOrigin (List<Runtime.ListDefinition> lists)
        {
            _lists = new Dictionary<string, ListDefinition> ();
            foreach (var list in lists) {
                _lists [list.name] = list;
            }
        }

        public bool TryGetDefinition (string name, out ListDefinition def)
        {
            return _lists.TryGetValue (name, out def);
        }

        public ListValue FindSingleItemListWithName (string name)
        {
            RawListItem item = RawListItem.Null;
            ListDefinition list = null;

            // Name could be in the form itemName or listName.itemName
            var nameParts = name.Split ('.');
            if (nameParts.Length == 2) {
                item = new RawListItem (nameParts [0], nameParts [1]);
                TryGetDefinition (item.originName, out list);
            } else {
                foreach (var namedList in _lists) {
                    var listWithItem = namedList.Value;
                    item = new RawListItem (namedList.Key, name);
                    if (listWithItem.ContainsItem (item)) {
                        list = listWithItem;
                        break;
                    }
                }
            }

            // Manager to get the list that contains the given item?
            if (list != null) {
                int itemValue = list.ValueForItem (item);
                return new ListValue (item, itemValue);
            }

            return null;
        }

        Dictionary<string, Runtime.ListDefinition> _lists;
    }
}
