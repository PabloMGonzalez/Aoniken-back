-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 15-11-2022 a las 02:38:26
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `aonikendb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comment`
--

CREATE TABLE `comment` (
  `id` int(11) NOT NULL,
  `content` varchar(255) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `post_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `comment`
--

INSERT INTO `comment` (`id`, `content`, `user_id`, `post_id`) VALUES
(1, '1º COMENTARIO DEL 1º POST', 4, 1),
(2, '1º COMENTARIO DEL 2º POST', 4, 2),
(3, '1º COMENTARIO DEL 4º POST', 4, 4),
(4, '2º COMENTARIO DEL 1º POST', 5, 1),
(5, '2º COMENTARIO DEL 2º POST', 5, 2),
(6, '2º COMENTARIO DEL 4º POST', 5, 4),
(7, '3º COMENTARIO DEL EDITOR', 2, 1),
(8, '3º COMENTARIO DEL EDITOR', 2, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `post`
--

CREATE TABLE `post` (
  `id` int(11) NOT NULL,
  `title` varchar(50) NOT NULL,
  `content` varchar(255) NOT NULL,
  `submit_date` date NOT NULL,
  `pending_approval` int(1) NOT NULL DEFAULT 0,
  `approve_date` date DEFAULT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `post`
--

INSERT INTO `post` (`id`, `title`, `content`, `submit_date`, `pending_approval`, `approve_date`, `user_id`) VALUES
(1, '1º POST', '1º CUERPO DEL PRIMER POST', '2022-11-14', 2, '2022-11-14', 1),
(2, '2º POST', '2º CUERPO DEL SEGUNDO POST', '2022-11-14', 2, '2022-11-14', 1),
(3, 'ESTE POST FUE EDITADO', 'ESTE POST FUE EDITADO', '2022-11-14', 2, '2022-11-14', 1),
(4, 'ESTE POST FUE EDITADO', 'POST EDITADO', '2022-11-14', 0, NULL, 1),
(5, '5º  POST', '5º CUERPO DEL QUINTO POST', '2022-11-14', 1, NULL, 1),
(6, '6º POST ', '6º CUERPO DEL SEXTO POST', '2022-11-14', 1, NULL, 1),
(7, '7º POST', '7º CUERPO DEL SEPTIMO POST', '2022-11-14', 0, NULL, 1),
(8, '8º POST', '8º CUERPO DEL OCTAVO POST', '2022-11-14', 0, NULL, 1),
(9, '9º POST', '9º CUERPO DEL NOVENO POST', '2022-11-14', 0, NULL, 1),
(10, '10º  POST', '10º CUERPO DEL DECIMO POST', '2022-11-14', 1, NULL, 1),
(11, '1º POST CON OTRO USUARIO', '1º POST CUERPO CON OTRO USUARIO', '2022-11-14', 0, NULL, 4),
(12, '2º POST CON OTRO USUARIO', '2º POST CUERPO CON OTRO USUARIO', '2022-11-14', 1, NULL, 4),
(13, '3º POST CON OTRO USUARIO', '3º POST CUERPO CON OTRO USUARIO', '2022-11-14', 2, '2022-11-14', 4),
(14, '4º POST CON OTRO USUARIO', '4º POST CUERPO CON OTRO USUARIO', '2022-11-14', 0, NULL, 3),
(15, '5º POST CON OTRO USUARIO', '5º POST CUERPO CON OTRO USUARIO', '2022-11-14', 2, '2022-11-14', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  `email` varchar(50) NOT NULL,
  `password` varchar(20) NOT NULL,
  `role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `user`
--

INSERT INTO `user` (`id`, `nombre`, `email`, `password`, `role`) VALUES
(1, 'writer', 'writer@writer.com', 'pass', 2),
(2, 'editor', 'editor@editor.com', 'pass', 1),
(3, 'admin', 'admin@admin.com', 'pass', 0),
(4, 'Pablo', 'p@p.com', 'pass', 2),
(5, 'Alejandro', 'a@a.com', 'pass', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `comment`
--
ALTER TABLE `comment`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `post_id` (`post_id`);

--
-- Indices de la tabla `post`
--
ALTER TABLE `post`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indices de la tabla `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `comment`
--
ALTER TABLE `comment`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `post`
--
ALTER TABLE `post`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `comment_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `post` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `comment_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`);

--
-- Filtros para la tabla `post`
--
ALTER TABLE `post`
  ADD CONSTRAINT `post_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
