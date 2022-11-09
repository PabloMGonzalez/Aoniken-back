-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 08-11-2022 a las 23:40:45
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
  `user_id` int(11) NOT NULL,
  `post_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `comment`
--

INSERT INTO `comment` (`id`, `content`, `user_id`, `post_id`) VALUES
(1, 'LOREPIASDHASFHJPFJAPdSAD', 3, 9),
(2, 'lo q sea x2', 3, 9),
(3, 'asfsdfsdf\r\n\r\njejejej', 2, 9),
(4, 'lolololollol', 5, 10),
(5, 'wowlwowolwlowolw', 10, 4),
(7, 'HOLA ESTE ES EL COMENTARIO DEL POST 17 DEL USUARIO 6, manejalo', 6, 17);

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
(4, 'SEGUNDO POST', 'SEGUNDO POST', '0000-00-00', 2, '2022-11-07', 5),
(9, 'post post post', 'post post post', '0000-00-00', 1, '2022-11-07', 6),
(10, 'post post postq32421vsdf', 'sdfsdfsdfdsfsdf', '0000-00-00', 0, '2022-11-07', 12),
(12, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 0, NULL, 3),
(13, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 0, NULL, 3),
(14, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, NULL, 3),
(15, 'TESTEEEEEEE EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 0, NULL, 3),
(16, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, NULL, 3),
(17, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, NULL, 3),
(18, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 0, NULL, 3),
(19, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '0000-00-00', 0, NULL, 3),
(20, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, NULL, 3);

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
(1, '', 'pm.gonzaalez@gmail.com', 'sarasa09', 0),
(2, '', 'p@p.com', 'sarasa09', 1),
(3, '', 'p@g.com', 'sarasa09', 2),
(4, '', 'pablo@gonzalez.com', 'sarasa09', 0),
(5, '', 'email@email.com', 'sarasa09', 1),
(6, '', 'hola@hola.com', 'sarasa09', 1),
(7, '', 'cake@cake.com', 'sarasa09', 2),
(8, '', 'm@m.com', 'sarasa09', 1),
(9, '', 'g@g.com', 'sarasa09', 1),
(10, '', 'l@lol.com', 'sarasa09', 2),
(11, '', 'ultimo@u.com', 'sarasa09', 1),
(12, '', 'asd@asd.com', 'sarasa09', 2),
(13, '', '', '', 2),
(14, '', 'test@test.com', 'sarasa09', 2);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `post`
--
ALTER TABLE `post`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

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
