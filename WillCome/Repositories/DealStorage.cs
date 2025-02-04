using System;
using System.Collections.ObjectModel;
using WillCome.Core.Database;
using WillCome.Core.Enums;
using WillCome.Core.Services;
using WillCome.Models.Deal;

namespace WillCome.Repositories
{
    // Хранилище данных для работы со сделками и воронками
    // TODO (крайняя точка разработки)
    public class DealStorage : IDealStorage
    {
        #region Fields

        private readonly IDBConnection _dbConnection;
        private readonly IUserAccessData _userAccessData;
        private static CoreDeal _coreDeal = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация хранилища сделок и загрузка данных по воронкам.
        /// </summary>
        public DealStorage(IDBConnection dbConnection, IUserAccessData userAccessData)
        {
            _dbConnection = dbConnection;
            _userAccessData = userAccessData;

            _coreDeal = new CoreDeal();
            DownloadFunnel();
        }

        #endregion

        #region Public Methods

        public ReadOnlyObservableCollection<Funnel> GetFunnels()
        {
            return _coreDeal.FunnelDataPublicValues;
        }

        public void LoadFunnel()
        {
            DownloadFunnel();
        }

        public void UpdateFunnel(Funnel funnel)
        {
            // TODO: Реализовать логику обновления воронки в базе данных
        }

        public void LoadDeals()
        {
            // TODO: Реализовать логику загрузки сделок из базы данных
        }

        public void UpdateDeals()
        {
            // TODO: Реализовать логику обновления сделок в базе данных
        }

        public void UpdatePositionStageDeal(Deal dealValue, Stage stageValue)
        {
            // TODO: Реализовать логику перемещения сделки на другой этап воронки
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Загружает данные о воронках (демонстрационные данные для тестирования).
        /// </summary>
        private void DownloadFunnel()
        {
            var testFunnel = new Funnel
            {
                Identifier = 1,
                Name = "Продажи",
                Description = "Воронка для управления продажами",
                Position = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            var stage1 = new Stage
            {
                Identifier = 1,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "Новая заявка",
                Position = 1,
                Color = 0,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 1,
                DealName = "Сделка #0",
                Budget = 15000m,
                CreateDate = DateTime.Now.AddDays(-1),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 201,
                ResponsibleFirstName = "Иван",
                ResponsibleLastName = "Петров",
                ContactType = EntitiesTypeEnum.Individual,
                ContactName = "Альбина Хазина",
                ContactIdentifier = 1001,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 0
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 1,
                DealName = "Сделка #1",
                Budget = 15000m,
                CreateDate = DateTime.Now.AddDays(-3),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 201,
                ResponsibleFirstName = "Иван",
                ResponsibleLastName = "Петров",
                ContactType = EntitiesTypeEnum.Individual,
                ContactName = "Анатолий Чупшев",
                ContactIdentifier = 1001,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 0
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 2,
                DealName = "Сделка #2",
                Budget = 20000m,
                CreateDate = DateTime.Now.AddDays(-2),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 202,
                ResponsibleFirstName = "Анна",
                ResponsibleLastName = "Сидорова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "Сергей Ждан",
                ContactIdentifier = 1002,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 5
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 2,
                DealName = "Сделка #2",
                Budget = 20000m,
                CreateDate = DateTime.Now.AddDays(-2),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 202,
                ResponsibleFirstName = "Анна",
                ResponsibleLastName = "Сидорова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "Сергей Ждан",
                ContactIdentifier = 1002,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 5
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 2,
                DealName = "Сделка #2",
                Budget = 20000m,
                CreateDate = DateTime.Now.AddDays(-2),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 202,
                ResponsibleFirstName = "Анна",
                ResponsibleLastName = "Сидорова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "Сергей Ждан",
                ContactIdentifier = 1002,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 5
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 2,
                DealName = "Сделка #2",
                Budget = 20000m,
                CreateDate = DateTime.Now.AddDays(-2),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 202,
                ResponsibleFirstName = "Анна",
                ResponsibleLastName = "Сидорова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "Сергей Ждан",
                ContactIdentifier = 1002,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 5
            });

            stage1.Deals.Add(new Deal
            {
                DealIdentifier = 2,
                DealName = "Сделка #2",
                Budget = 20000m,
                CreateDate = DateTime.Now.AddDays(-2),
                Funnel = testFunnel.Identifier,
                Stage = stage1.Name,
                ResponsibleIdentifier = 202,
                ResponsibleFirstName = "Анна",
                ResponsibleLastName = "Сидорова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "Сергей Ждан",
                ContactIdentifier = 1002,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 5
            });

            var stage2 = new Stage
            {
                Identifier = 2,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "Переговоры",
                Position = 2,
                Color = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            stage2.Deals.Add(new Deal
            {
                DealIdentifier = 3,
                DealName = "Сделка #3",
                Budget = 30000m,
                CreateDate = DateTime.Now.AddDays(-5),
                Funnel = testFunnel.Identifier,
                Stage = stage2.Name,
                ResponsibleIdentifier = 203,
                ResponsibleFirstName = "Олег",
                ResponsibleLastName = "Кузнецов",
                ContactType = EntitiesTypeEnum.Individual,
                ContactName = "Анна Виктровна",
                ContactIdentifier = 1003,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 1
            });

            var stage3 = new Stage
            {
                Identifier = 3,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "Согласование договора",
                Position = 3,
                Color = 2,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            stage3.Deals.Add(new Deal
            {
                DealIdentifier = 4,
                DealName = "Сделка #4",
                Budget = 50000m,
                CreateDate = DateTime.Now.AddDays(-7),
                Funnel = testFunnel.Identifier,
                Stage = stage3.Name,
                ResponsibleIdentifier = 204,
                ResponsibleFirstName = "Мария",
                ResponsibleLastName = "Кириллова",
                ContactType = EntitiesTypeEnum.Legal,
                ContactName = "ИП Н.О. Абармов",
                ContactIdentifier = 1004,
                ContactPhone = "+7 (919)-600-76-50",
                TaskSummary = 3
            });

            var stage4 = new Stage
            {
                Identifier = 4,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "Закрытие сделки",
                Position = 4,
                Color = 3,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            stage4.Deals.Add(new Deal
            {
                DealIdentifier = 5,
                DealName = "Сделка #5",
                Budget = 75000m,
                CreateDate = DateTime.Now.AddDays(-10),
                Funnel = testFunnel.Identifier,
                Stage = stage4.Name,
                ResponsibleIdentifier = 205,
                ResponsibleFirstName = "Алексей",
                ResponsibleLastName = "Смирнов",
                ContactType = EntitiesTypeEnum.None,
                TaskSummary = 0
            });

            var stage5 = new Stage
            {
                Identifier = 5,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "+1 этап",
                Position = 4,
                Color = 14,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            var stage6 = new Stage
            {
                Identifier = 5,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "+1 этап",
                Position = 4,
                Color = 14,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            var stage7 = new Stage
            {
                Identifier = 5,
                FunnelIdentifier = testFunnel.Identifier,
                Name = "+1 этап",
                Position = 4,
                Color = 14,
                CreatedAt = DateTime.Now,
                CreatedBy = 101
            };

            testFunnel.Stages.Add(stage1);
            testFunnel.Stages.Add(stage2);
            testFunnel.Stages.Add(stage3);
            testFunnel.Stages.Add(stage4);
            testFunnel.Stages.Add(stage5);
            testFunnel.Stages.Add(stage6);
            testFunnel.Stages.Add(stage7);

            _coreDeal.AppendFunnelData(testFunnel);
        }

        #endregion
    }
}
