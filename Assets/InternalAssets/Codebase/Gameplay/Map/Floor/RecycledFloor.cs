using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Random;
using Codebase.Library.SAD;
using Sirenix.OdinInspector;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Map.Floor
{
    public class RecycledFloor : MonoBehaviour
    {
        [SerializeField, ReadOnly] private float _horizontalOffset = 0;
        [SerializeField, ReadOnly] private float _verticalOffset = 0;
        
        [BoxGroup("Offset params"), SerializeField] private RecycledFloorOffsetParams _offsetParams;
        [BoxGroup("Install params"), SerializeField] private RecycledFloorInstallParams _installParams;
        [BoxGroup("Style params"), SerializeField] private RecycledFloorStyleParams _styleParams;
        
        [BoxGroup("Siblings"), SerializeField, ReadOnly] private RecycledFloorElement[] _siblings = Array.Empty<RecycledFloorElement>();

        private RecycledFloorElement[,] _floorElements;
        private IDisposable _floorUpdatingDisposable;
        private Entity _listeningEntity;

        private float _lastListenedX = 0f;
        private float _lastListenedY = 0f;
        
        [Button]
        public void Initialize(Entity listeningEntity)
        {
            _floorElements = new RecycledFloorElement[_installParams.Length, _installParams.Width];

            _listeningEntity = listeningEntity;

            for (int i = 0, pickedSiblingIndex = 0; i < _installParams.Length; i++)
            {
                for (int j = 0; j < _installParams.Width; j++)
                {
                    _floorElements[i, j] = _siblings[pickedSiblingIndex];
                    pickedSiblingIndex++;
                }
            }
            
            _floorUpdatingDisposable?.Dispose();
            _floorUpdatingDisposable = Observable.EveryUpdate().Subscribe(_ => UpdateValues());
        }

        private void OnDestroy()
        {
            _floorUpdatingDisposable?.Dispose();
        }

        private void UpdateValues()
        {
            if(_listeningEntity == null) return;

            Vector3 entityWorldPosition = _listeningEntity.Transform.position;

            _horizontalOffset += entityWorldPosition.x - _lastListenedX;
            _verticalOffset += entityWorldPosition.y - _lastListenedY;

            _lastListenedX = entityWorldPosition.x;
            _lastListenedY = entityWorldPosition.y;
            
            if(_horizontalOffset <= _offsetParams.MinOffsetX) ShiftLeft();
            if(_horizontalOffset >= _offsetParams.MaxOffsetX) ShiftRight();
            if(_verticalOffset <= _offsetParams.MinOffsetY) ShiftDown();
            if(_verticalOffset >= _offsetParams.MaxOffsetY) ShiftUp();
        }

        private void ShiftRight()
        {
            _horizontalOffset -= _installParams.Offset;
            
            for (int i = 0; i < _installParams.Length; i++)
            {
                RecycledFloorElement edgeElement = _floorElements[i, 0];

                for (int j = 1; j < _installParams.Width; j++)
                {
                    _floorElements[i, j - 1] = _floorElements[i, j];

                    if (j + 1 == _installParams.Width)
                    {
                        Vector3 currentElementPosition = _floorElements[i, j].Transform.position;
                        
                        (_floorElements[i, j] = edgeElement).Transform.position = new Vector3(
                            x: currentElementPosition.x + _installParams.Offset,
                            y: currentElementPosition.y,
                            z: 0
                            );
                    }
                }
                
                ChangeElementSprite(edgeElement);
            }
        }

        private void  ShiftLeft()
        {
            _horizontalOffset += _installParams.Offset;
            
            for (int i = 0; i < _installParams.Length; i++)
            {
                RecycledFloorElement edgeElement = _floorElements[i, _installParams.Width - 1];

                for (int j = _installParams.Width - 2; j >= 0; j--)
                {
                    _floorElements[i, j + 1] = _floorElements[i, j];

                    if (j == 0)
                    {
                        Vector3 currentElementPosition = _floorElements[i, j].Transform.position;
                        
                        (_floorElements[i, j] = edgeElement).Transform.position = new Vector3(
                            x: currentElementPosition.x - _installParams.Offset,
                            y: currentElementPosition.y,
                            z: 0
                        );
                    }
                }
                
                ChangeElementSprite(edgeElement);
            }
        }

        private void ShiftUp()
        {
            _verticalOffset -= _installParams.Offset;

            for (int j = 0; j < _installParams.Width; j++)
            {
                RecycledFloorElement edgeElement = _floorElements[_installParams.Length - 1, j];
                
                for (int i = _installParams.Length - 2; i >= 0; i--)
                {
                    _floorElements[i + 1, j] = _floorElements[i, j];

                    if (i == 0)
                    {
                        Vector3 currentElementPosition = _floorElements[i, j].Transform.position;
                        
                        (_floorElements[i, j] = edgeElement).Transform.position = new Vector3(
                            x: currentElementPosition.x,
                            y: currentElementPosition.y + _installParams.Offset,
                            z: 0
                        );
                    }
                }
                
                ChangeElementSprite(edgeElement);
            }
        }

        private void ShiftDown()
        {
            _verticalOffset += _installParams.Offset;

            for (int j = 0; j < _installParams.Width; j++)
            {
                RecycledFloorElement edgeElement = _floorElements[0, j];
                
                for (int i = 1; i < _installParams.Length; i++)
                {
                    _floorElements[i - 1, j] = _floorElements[i, j];

                    if (i + 1 == _installParams.Length)
                    {
                        Vector3 currentElementPosition = _floorElements[i, j].Transform.position;
                        
                        (_floorElements[i, j] = edgeElement).Transform.position = new Vector3(
                            x: currentElementPosition.x,
                            y: currentElementPosition.y - _installParams.Offset,
                            z: 0
                        );
                    }
                }

                ChangeElementSprite(edgeElement);
            }
        }

        private void ChangeElementSprite(RecycledFloorElement element) => 
            element.Renderer.sprite = _styleParams.StyleSprites.Random();

#if UNITY_EDITOR

        [Button]
        private void ReCreate()
        {
            List<Transform> internalObjects = transform.GetComponentsInChildren<Transform>().ToList();

            foreach (Transform obj in internalObjects)
            {
                if(obj == transform)
                    continue;
                
                DestroyImmediate(obj.gameObject);
            }
            
            float currentPosY = _installParams.StartPositionY;
            
            for (int i = 0; i < _installParams.Length; i++)
            {
                float currentPosX = _installParams.StartPositionX;
                
                for (int j = 0; j < _installParams.Width; j++)
                {
                    RecycledFloorElement element = PrefabUtility.InstantiatePrefab(_installParams.FloorElementPrefab, transform) as RecycledFloorElement;
                    
                    element.GameObject.name = "FloorElement";
                    element.Transform.position = new Vector3(currentPosX, currentPosY, 0);

                    ChangeElementSprite(element);
                    
                    currentPosX += _installParams.Offset;
                }
                
                currentPosY -= _installParams.Offset;
            }
        }

        [Button]
        private void CacheSiblings() => 
            _siblings = transform.GetComponentsInChildren<RecycledFloorElement>();

#endif
    }
    
    [Serializable]
    public class RecycledFloorOffsetParams
    {
        [field: SerializeField] public float MinOffsetX { get; private set; }
        [field: SerializeField] public float MaxOffsetX { get; private set; }
        [field: SerializeField] public float MinOffsetY { get; private set; }
        [field: SerializeField] public float MaxOffsetY { get; private set; }
    }
    
    [Serializable]
    public class RecycledFloorInstallParams
    {
        [field: SerializeField] public RecycledFloorElement FloorElementPrefab { get; private set;}
        [field: SerializeField] public int Width { get; private set;}
        [field: SerializeField] public int Length { get; private set;}
        [field: SerializeField] public float StartPositionX { get; private set;}
        [field: SerializeField] public float StartPositionY { get; private set;}
        [field: SerializeField] public float Offset { get; private set;}
    }
    
    [Serializable]
    public class RecycledFloorStyleParams
    {
        [field: SerializeField] public Sprite[] StyleSprites { get; private set; }
    }
}
