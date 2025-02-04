using System;
using System.Linq;
using WillCome.Models.Deal;
using WillCome.Repositories;
using WillCome.Utils;
using GongSolutions.Wpf.DragDrop;
using ModalWindow;
using WillCome.ModalWindows;
using System.Windows;

namespace WillCome.ViewModels
{
    // ViewModel для работы с представлением "Сделки"
    public class DealViewModel : WorkspaceViewModel, IDropTarget
    {
        #region Fields

        // Хранилище данных сделок
        private readonly IDealStorage _dealStorage;
        // Сервис для работы с модальными окнами
        private readonly IDialogService _dialogService;

        // Текущая выбранная воронка
        private Funnel _selectedFunnel;                   

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация DealViewModel с сервисами хранения и диалогов.
        /// </summary>
        public DealViewModel(IDealStorage dealStorage, IDialogService dialogService)
        {
            _dealStorage = dealStorage;
            _dialogService = dialogService;

            SelectedFunnel = _dealStorage.GetFunnels().FirstOrDefault();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Текущая выбранная воронка для отображения сделок.
        /// </summary>
        public Funnel SelectedFunnel
        {
            get => _selectedFunnel;
            set
            {
                if (_selectedFunnel != value)
                {
                    _selectedFunnel = value;
                    OnPropertyChanged(nameof(SelectedFunnel));
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Команда для добавления нового этапа в воронку.
        /// </summary>
        public RelayCommand AddStageCommand => new RelayCommand(param =>
        {
            bool? result = _dialogService.ShowDialog<AddStageInFunnelWindowViewModel>();
        });

        #endregion

        #region Drag-and-Drop Implementation (IDropTarget)

        public void DragEnter(IDropInfo dropInfo) { }

        public void DragLeave(IDropInfo dropInfo) { }
        public void DropHint(IDropInfo dropInfo) { }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceDeal = dropInfo.Data as Deal;
            var targetStage = dropInfo.TargetItem as Stage;

            if (sourceDeal != null && targetStage != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
            else
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceDeal = dropInfo.Data as Deal;
            var targetStage = dropInfo.TargetItem as Stage;
            var sourceStage = GetSourceStageForDeal(sourceDeal);

            if (sourceDeal != null && targetStage != null && sourceStage != null && targetStage != sourceStage)
            {
                sourceStage.Deals.Remove(sourceDeal);
                targetStage.Deals.Add(sourceDeal);
                sourceDeal.Stage = targetStage.Name;

                OnPropertyChanged(nameof(SelectedFunnel));
            }
        }

        private Stage GetSourceStageForDeal(Deal deal)
        {
            return SelectedFunnel?.Stages?.FirstOrDefault(stage => stage.Deals.Contains(deal));
        }

        #endregion

        #region Base Class Overrides

        public override string DisplayName => "Сделки";

        public override void UpdateModule()
        {
            // TO-DO: Реализовать логику обновления данных
        }

        public void DropHint(IDropHintInfo dropHintInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}