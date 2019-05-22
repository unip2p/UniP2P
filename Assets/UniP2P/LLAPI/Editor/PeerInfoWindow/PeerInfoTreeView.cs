using System;
using System.Collections.Generic;
using System.Linq;
using UniP2P.LLAPI;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UniP2P
{

    public class PeerInfoItem : TreeViewItem
    {
        public Peer element { get; set; }

        public PeerInfoItem(int id, Peer peer) : base(id)
        {
            element = peer;
        }
    }

    public class PeerInfoTreeView : TreeView
    {
        private const string sortedColumnIndexStateKey = "PeerInfoTreeView_sortedColumnIndex";

        public IReadOnlyList<TreeViewItem> CurrentBindingItems;

        public PeerInfoTreeView()
            : this(new TreeViewState(), new MultiColumnHeader(new MultiColumnHeaderState(new[]
            {
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent(" "), width = 16},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("PeerID"), width = 250},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("IPEndPoint"), width = 200 }
            })))
        {
        }

        PeerInfoTreeView(TreeViewState state, MultiColumnHeader header)
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
            foreach (var peer in UniP2PManager.GetAllPeer())
            {
                children.Add(new PeerInfoItem(index++, peer));
            }

            CurrentBindingItems = children;
            rootItem.children = CurrentBindingItems as List<TreeViewItem>;
            return rootItem;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var items = (PeerInfoItem)args.item;

            for (var visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); visibleColumnIndex++)
            {
                var rect = args.GetCellRect(visibleColumnIndex);
                var columnIndex = (ColumnIndex)args.GetColumn(visibleColumnIndex);

                var labelStyle = args.selected ? EditorStyles.whiteLabel : EditorStyles.label;
                labelStyle.alignment = TextAnchor.MiddleLeft;
                switch (columnIndex)
                {
                    case ColumnIndex.SortId:
                        EditorGUI.LabelField(rect, args.item.id.ToString(), labelStyle);
                        break;
                    case ColumnIndex.PeerId:
                        EditorGUI.LabelField(rect, items.element.ID, labelStyle);
                        break;
                    case ColumnIndex.IPEndPoint:
                        if (items.element.IPEndPoint != null)
                        {
                            EditorGUI.LabelField(rect, items.element.IPEndPoint.ToString(), labelStyle);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, null);
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            /*    var textureTableItem = (PeerInfoItem)FindItem(id, rootItem);
                EditorGUIUtility.PingObject(textureTableItem?.element.icon);*/
        }

        public void SortItems(MultiColumnHeader multiColumnHeader)
        {
            SessionState.SetInt(sortedColumnIndexStateKey, multiColumnHeader.sortedColumnIndex);
            var index = (ColumnIndex)multiColumnHeader.sortedColumnIndex;
            var ascending = multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex);

            var items = rootItem.children.Cast<PeerInfoItem>();

            IOrderedEnumerable<PeerInfoItem> orderedEnumerable;

            switch (index)
            {
                case ColumnIndex.SortId:
                    orderedEnumerable = items.OrderBy(item => item.id);
                    break;
                case ColumnIndex.PeerId:
                    orderedEnumerable = items.OrderBy(item => item.element.ID);
                    break;
                case ColumnIndex.IPEndPoint:
                    orderedEnumerable = items.OrderBy(item => item.element.IPEndPoint.ToString());
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

    public enum ColumnIndex
    {
        SortId,
        PeerId,
        IPEndPoint
    }
}