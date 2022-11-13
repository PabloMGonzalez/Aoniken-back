-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 13-11-2022 a las 22:42:25
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 8.1.10

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
(51, 'hola', 2, 10),
(52, 'soy pepe', 2, 10),
(58, 'hola soy capo, y saque esto con localstorage, me falta el reverse de mierda', 2, 10),
(59, '', 2, 10),
(60, '', 2, 10);

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
(9, 'post post post', 'post post post', '0000-00-00', 1, '2022-11-09', 6),
(10, 'post post postq32421vsdf', 'sdfsdfsdfdsfsdf', '0000-00-00', 2, '2022-11-13', 12),
(12, 'GGGGGGGGGGGGGGG', 'G', '2022-11-10', 2, '2022-11-13', 3),
(13, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 2, '2022-11-13', 3),
(14, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, '2022-11-13', 3),
(15, 'DAPPER EDIT', 'EEDDDIT WITH DAPPER11!!', '0001-01-01', 0, '2022-11-13', 3),
(16, 'edito DE NUEVO CON DAPPER', 'A VER ESA FECHITA DAPPER', '2022-11-13', 0, '2022-11-13', 3),
(17, 'TITULO DEL POST', 'CONTENIDO DEL POST', '2022-11-07', 0, '2022-11-13', 3),
(18, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '2022-11-07', 0, '2022-11-13', 3),
(19, 'TEST EDIT', 'ACA VA UN RE POST RE COPADO CON UN MONTON DE INFO Q ACABO DE EDITAR NO SABES CHABON!!!!', '0000-00-00', 0, '2022-11-13', 3),
(24, 'esto anda bien ejej', 'jeejje', '2022-11-12', 0, '2022-11-13', 2),
(25, 'dapperdapper', 'dappppeeeeeeeeeerrrrrrrrrrrrrrr', '0001-01-01', 0, '2022-11-13', 3),
(26, 'a ver dappper con las fechas', 'dapper y fechas', '0001-01-01', 2, '2022-11-13', 3),
(27, 'asdad', 'fsfdsdf', '0001-01-01', 2, '2022-11-13', 3),
(28, 'DAAAPPPERR', 'DAAAPPPER OLE OLE', '2022-11-13', 0, '2022-11-13', 3),
(29, 'dapper esta bueno me gusta dapper', 'dapper dapper ole ole', '2022-11-13', 0, '2022-11-13', 3);

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
(2, 'CAPO', 'p@p.com', 'sarasa09', 1),
(3, 'GSU', 'p@g.com', 'sarasa09', 2),
(4, '', 'pablo@gonzalez.com', 'sarasa09', 0),
(5, 'Pablo', 'email@email.com', 'sarasa09', 1),
(6, 'MARCOS', 'hola@hola.com', 'sarasa09', 1),
(7, '', 'cake@cake.com', 'sarasa09', 2),
(8, '', 'm@m.com', 'sarasa09', 1),
(9, '', 'g@g.com', 'sarasa09', 1),
(10, '', 'l@lol.com', 'sarasa09', 2),
(11, '', 'ultimo@u.com', 'sarasa09', 1),
(12, 'catriel', 'asd@asd.com', 'sarasa09', 2),
(13, '', '', '', 2),
(14, '', 'test@test.com', 'sarasa09', 2),
(16, 'Jose', 'j@j.com', 'sarasa09', 2),
(18, 'Joise', 'ggg@ggg.com', 'sarasa09', 2);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=61;

--
-- AUTO_INCREMENT de la tabla `post`
--
ALTER TABLE `post`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

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
