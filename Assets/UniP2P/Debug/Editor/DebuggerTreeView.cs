using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UniP2P.Debug
{
    public class DebuggerItem : TreeViewItem
    {
        public DebbugerMessage element;

        public DebuggerItem(int id, DebbugerMessage dm) : base(id)
        {
            element = dm;
        }
    }

    public class DebuggerTreeView : TreeView
    {
        private const string sortedColumnIndexStateKey = "UDPConnectionTreeView_sortedColumnIndex";

        public IReadOnlyList<TreeViewItem> CurrentBindingItems;

        public DebuggerTreeView()
            : this(new TreeViewState(), new MultiColumnHeader(new MultiColumnHeaderState(new[]
            {
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent(" "), width = 22},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Message"), width = 400},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Type"), width = 150},
            })))
        {
        }

        DebuggerTreeView(TreeViewState state, MultiColumnHeader header)
            : base(state, header)
        {
            rowHeight = 20;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            header.sortingChanged += SortItems;

            header.ResizeToFit();
            Reload();

            header.sortedColumnIndex = SessionState.GetInt(sortedColumnIndexStateKey, 1);
        }

        public void ReloadAndSort()
        {
            var currentSelected = this.state.selectedIDs;
            Reload();
            SortItems(this.multiColumnHeader);
            this.state.selectedIDs = currentSelected;
        }

        protected override TreeViewItem BuildRoot()
        {
            var rootItem = new TreeViewItem
            {
                depth = -1
            };

            var children = new List<TreeViewItem>();

            var index = 1;
            foreach (var mes in DebbugerMessages.Messages)
            {
                children.Add(new DebuggerItem(index++, mes));
            }

            CurrentBindingItems = children;
            rootItem.children = CurrentBindingItems as List<TreeViewItem>;
            return rootItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var items = (DebuggerItem)args.item;

            for (var visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); visibleColumnIndex++)
            {
                var rect = args.GetCellRect(visibleColumnIndex);
                var columnIndex = (DebuggerColumnIndex)args.GetColumn(visibleColumnIndex);

                var labelStyle = args.selected ? EditorStyles.whiteLabel : EditorStyles.label;
                labelStyle.alignment = TextAnchor.MiddleLeft;
                switch (columnIndex)
                {
                    case DebuggerColumnIndex.SortId:
                        EditorGUI.SelectableLabel(rect, args.item.id.ToString(), labelStyle);
                        break;
                    case DebuggerColumnIndex.Message:
                        EditorGUI.SelectableLabel(rect, items.element.Message, labelStyle);
                        break;
                    case DebuggerColumnIndex.Type:
                        EditorGUI.SelectableLabel(rect, items.element.Type.ToString(), labelStyle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, null);
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
           
            
        }

        public void SortItems(MultiColumnHeader multiColumnHeader)
        {
            SessionState.SetInt(sortedColumnIndexStateKey, multiColumnHeader.sortedColumnIndex);
            var index = (DebuggerColumnIndex)multiColumnHeader.sortedColumnIndex;
            var ascending = multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex);

            var items = rootItem.children.Cast<DebuggerItem>();

            IOrderedEnumerable<DebuggerItem> orderedEnumerable;

            switch (index)
            {
                case DebuggerColumnIndex.SortId:
                    orderedEnumerable = items.OrderBy(item => item.id);
                    break;
                case DebuggerColumnIndex.Message:
                    orderedEnumerable = items.OrderBy(item => item.element.Message);
                    break;
                case DebuggerColumnIndex.Type:
                    orderedEnumerable = items.OrderBy(item => item.element.Type.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }

            items = orderedEnumerable.AsEnumerable();

            if (!ascending)
            {
                items = items.Reverse();
            }

            rootItem.children = items.Cast<TreeViewItem>().ToList();
            BuildRows(rootItem);
        }
    }

    public enum DebuggerColumnIndex
    {
        SortId,
        Message,
        Type
    }
}