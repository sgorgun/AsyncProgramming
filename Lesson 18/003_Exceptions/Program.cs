﻿var parent = new Task(() =>
{
    new Task(() =>
    {
        Thread.Sleep(500);
        Console.WriteLine("Вложенная задача #1 завершила свою работу");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(600);
        Console.WriteLine("Вложенная задача #2 завершила свою работу");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(700);
        throw new Exception("Ошибка в вложенной задаче #3");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(800);
        Console.WriteLine("Вложенная задача #4 завершила свою работу");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        new Task(() => throw new Exception("Ошибка в вложенной задаче #5.1 второго уровня вложенности"), TaskCreationOptions.AttachedToParent).Start();
        Thread.Sleep(900);
        throw new Exception("Ошибка в вложенной задаче #5");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(1000);
        Console.WriteLine("Вложенная задача #6 завершила свою работу");
    }, TaskCreationOptions.AttachedToParent).Start();
});

parent.Start();

try
{
    parent.Wait();
}
catch (AggregateException ex)
{
    Console.WriteLine();
    foreach (var item in ex.InnerExceptions)
    {
        if (item is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                Console.WriteLine($"Сообщение из исключчения дочерней задачи - {innerException.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Сообщение из исключчения родительской задачи - {item.Message}");
        }
    }
    //HandleTaskExeption(ex);
}
Console.WriteLine(new string('*', 80));
Console.WriteLine($"Статус родительской задачи {parent.Status}");
Console.WriteLine("Нажмите любую клавишу для завершения.");
Console.ReadKey();

void HandleTaskExeption(AggregateException exception)
{
    foreach (var item in exception.InnerExceptions)
    {
        if (item is AggregateException aggregateException)
        {
            HandleTaskExeption(aggregateException);
        }
        else
        {
            Console.WriteLine($"Сообщение из исключения - {item.Message}");
        }
    }
}
