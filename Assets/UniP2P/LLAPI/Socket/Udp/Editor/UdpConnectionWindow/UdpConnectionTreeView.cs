using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UniP2P.LLAPI;

namespace UniP2P
{

    public class UDPConnectionItem : TreeViewItem
    {
        public UdpConnection element { get; set; }

        public UDPConnectionItem(int id, UdpConnection con) : base(id)
        {
            element = con;
        }
    }

    public class UDPConnectionTreeView : TreeView
    {
        private const string sortedColumnIndexStateKey = "UDPConnectionTreeView_sortedColumnIndex";

        public IReadOnlyList<TreeViewItem> CurrentBindingItems;

        public UDPConnectionTreeView()
            : this(new TreeViewState(), new MultiColumnHeader(new MultiColumnHeaderState(new[]
            {
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent(" "), width = 16},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("PeerID"), width = 250},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("IPEndPoint"), width = 200},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("State"), width = 100},
            })))
        {
        }

        UDPConnectionTreeView(TreeViewState state, MultiColumnHeader header)
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
            foreach (var con in SocketUdp.GetUdpConnections())
            {
                children.Add(new UDPConnectionItem(index++, con));
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
            var items = (UDPConnectionItem)args.item;

            for (var visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); visibleColumnIndex++)
            {
                var rect = args.GetCellRect(visibleColumnIndex);
                var columnIndex = (udpColumnIndex)args.GetColumn(visibleColumnIndex);

                var labelStyle = args.selected ? EditorStyles.whiteLabel : EditorStyles.label;
                labelStyle.alignment = TextAnchor.MiddleLeft;
                switch (columnIndex)
                {
                    case udpColumnIndex.SortId:
                        EditorGUI.LabelField(rect, args.item.id.ToString(), labelStyle);
                        break;
                    case udpColumnIndex.PeerId:
                        EditorGUI.LabelField(rect, items.element.Peer.ID, labelStyle);
                        break;
                    case udpColumnIndex.IPEndPoint:
                        EditorGUI.LabelField(rect, items.element.Peer.IPEndPoint.ToString(), labelStyle);
                        break;
                    case udpColumnIndex.ConnectionState:
                        EditorGUI.LabelField(rect, items.element.State.ToString(), labelStyle);
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
            var index = (udpColumnIndex)multiColumnHeader.sortedColumnIndex;
            var ascending = multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex);

            var items = rootItem.children.Cast<UDPConnectionItem>();

            IOrderedEnumerable<UDPConnectionItem> orderedEnumerable;

            switch (index)
            {
                case udpColumnIndex.SortId:
                    orderedEnumerable = items.OrderBy(item => item.id);
                    break;
                case udpColumnIndex.PeerId:
                    orderedEnumerable = items.OrderBy(item => item.element.Peer.ID);
                    break;
                case udpColumnIndex.IPEndPoint:
                    orderedEnumerable = items.OrderBy(item => item.element.Peer.IPEndPoint.ToString());
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

    public enum udpColumnIndex
    {
        SortId,
        PeerId,
        IPEndPoint,
        ConnectionState
    }
}