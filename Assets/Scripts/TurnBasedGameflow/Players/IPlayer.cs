using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    // Интерфейс игрока для пошаговой системы
    string Name { get; }
    int ID { get; }

    // Эти две переменные нужны для гибкости подсчёта очков
    int Points { get; set; } // В почти ЛЮБОЙ пошаговой системе так или иначе присуствуют очки
    int OverallScore { get; set; } // В основном количество выигранных раундов
    // Актуальный архитектурный вопрос заменить int на float

    bool IsAI { get; }
    // Действие игрока
    // Для ии - начало размышлений над ходом
    // Для человека - возможность взаимодействовать с интерфейсом
    void Act();
}
