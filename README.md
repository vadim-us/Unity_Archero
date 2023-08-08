<h2>Задача:</h2> реализовать базовый геймплей игры в стиле Archero.
<h2>Технологии:</h2> Unity3D (версия 2020.2.3.F1), C#. 

<h2>Обязательные условия:</h2>
<ul>
<li>игровое поле заранее заданных размеров, вид сверху;</li>
<li>наличие 2х типов поведения противников;</li>
<li>наличие непроходимых по земле и непростреливаемых препятствий;</li>
<li>возможность управлять героем с помощью джойстика (движение);</li>
<li>герой должен получать монетки за убийство врагов;</li>
<li>камера на любых соотношениях сторон всегда охватывает всё игровое поле.</li>
</ul>
 
<h2> Дополнительный функционал (будет плюсом):</h2>
 <ul>
<li>эффекты попадания снарядов / смерти (формально);</li>
<li>несколько типов оружия игрока;</li>
<li>умные противники: каждое перемещение ставит или приближает моба к прямой видимости игроком;</li>
<li>меню/окно паузы (базовый функционал UI/окон);</li>
<li>дополнительные типы поведения противников.</li>
</ul>

<h1>Техническое задание </h1>

<h2>Описание геймплея:</h2> при старте игры происходит спаун противников в случайной области верхних 2/3 игрового поля и спаун игрока в центре нижней границы игрового поля. Сразу после расстановки идёт 3х-секундный отсчёт, после него начинается геймплей.

<br>Главный персонаж свободно перемещается по полю (с учётом препятствий) и стреляет снарядами во врагов - стрельба ведётся в автоматическом режиме, пока персонаж стоит на месте 
<br>Противники перемещаются согласно своей логике и также ведут стрельбу стоя. 
<br>При столкновении снаряда с противником или главным персонажем наносится урон, снаряд исчезает. При столкновении противника и персонажа, персонажу наносится урон, взаимопроникновение невозможно Главный персонаж: 
<br>Персонаж полностью подконтролен игроку, перемещается с помощью джойстика, во время передвижения он обращен лицевой стороной по вектору движения, после остановки он начинает слежение за ближайшим противником с помощью поворота вокруг своей оси и автоматическую стрельбу в его направлении. 

<h2>Характеристики персонажа: </h2>
 <ul>
<li>скорость передвижения; </li>
<li>количество HP;</li>
<li>скорость стрельбы - урон за выстрел. </li>
 </ul>
 
<h2>Противники:</h2> 
После старта игры противники включают логику поведения, она состоит из: 
 <ul>
<li>перемещения на более выгодную позицию; </li>
<li>стрельбы при условии неподвижности.</li>
    </ul>
Исходное состояние - неподвижность.

 
 <h2>Типы противников: </h2>
   <ul>
<li>наземный (перемещение блокируется препятствиями);</li>
<li>летающий (перемещается над препятствиями).</li>
   </ul>
  
<h2>Характеристики противников:</h2>
   <ul>
<li>скорость передвижения; </li>
<li>дальность передвижения; </li>
<li>время неподвижности;</li> 
<li>количество HP; </li>
<li>скорость стрельбы; </li>
<li>урон за выстрел. </li>
  </ul>
  
<h2>Цель игры:</h2> выжить, убить всех противников и выйти через «открытые двери» в верхней части уровня.