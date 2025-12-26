using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    // Интерфейс игрока для пошаговой системы
    string Name { get; }
    int ID { get; }
    int Points { get; } // В ЛЮБОЙ пошаговой системе так или иначе присуствуют очки
    bool IsAI { get; }
    // Действие игрока
    // Для ии - начало размышлений над ходом
    // Для человека - возможность взаимодействовать с интерфейсом
    void Act();
}
