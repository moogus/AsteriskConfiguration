delimiter $$

CREATE TABLE `ast_sipregs` (
  `ipaddr` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `port` varchar(5) DEFAULT NULL,
  `regseconds` bigint(20) DEFAULT NULL,
  `useragent` varchar(128) DEFAULT NULL,
  `lastms` int(11) DEFAULT NULL,
  `fullcontact` varchar(128) DEFAULT NULL,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `defaultuser` varchar(45) DEFAULT NULL,
  `regserver` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_4ComfederationLink` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `com_trunkId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=45 

delimiter $$

CREATE TABLE `fu_SamsungfederationLink` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `com_trunkId` int(11) DEFAULT NULL,
  `com_extensionsId` int(11) DEFAULT NULL,
  `com_routingRuleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_emergencyNumber` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `number` varchar(45) DEFAULT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_knownNumber` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `number` varchar(45) DEFAULT NULL,
  `description` varchar(45) DEFAULT NULL,
  `isInternal` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `number_index` (`number`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_musiconhold` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(80) NOT NULL,
  `directory` varchar(255) NOT NULL,
  `application` varchar(255) NOT NULL,
  `mode` varchar(80) NOT NULL DEFAULT 'files',
  `digit` char(1) NOT NULL,
  `sort` varchar(16) NOT NULL,
  `format` varchar(16) NOT NULL,
  `random` varchar(45) DEFAULT 'yes',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_permisionClassMember` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_dialplanId` int(11) NOT NULL DEFAULT '0',
  `permissionPatternId` int(11) NOT NULL DEFAULT '0',
  `permissionClassId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_permissionClass` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_permissionPattern` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_pattern` varchar(45) NOT NULL,
  `fu_name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `ast_voicemail` (
  `uniqueid` int(4) NOT NULL AUTO_INCREMENT,
  `customer_id` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `context` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `mailbox` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `password` int(4) NOT NULL,
  `fullname` varchar(150) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `pager` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `tz` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT 'central',
  `attach` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'yes',
  `saycid` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'yes',
  `dialout` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `callback` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `review` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `operator` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `envelope` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `sayduration` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `saydurationm` tinyint(4) NOT NULL DEFAULT '1',
  `sendvoicemail` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `delete` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `nextaftercmd` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'yes',
  `forcename` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `forcegreetings` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'no',
  `hidefromdir` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'yes',
  `stamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `attachfmt` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `searchcontexts` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `cidinternalcontexts` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `exitcontext` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `volgain` varchar(4) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `tempgreetwarn` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin DEFAULT 'yes',
  `messagewrap` enum('yes','no') CHARACTER SET utf8 COLLATE utf8_bin DEFAULT 'no',
  `minpassword` int(2) DEFAULT '4',
  `vm-password` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-newpassword` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-passchanged` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-reenterpassword` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-mismatch` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-invalid-password` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `vm-pls-try-again` varchar(10) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `listen-control-forward-key` varchar(2) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `listen-control-reverse-key` varchar(1) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `listen-control-pause-key` varchar(1) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `listen-control-restart-key` varchar(1) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `listen-control-stop-key` varchar(13) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `backupdeleted` varchar(3) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT '25',
  `maxsecs` int(11) DEFAULT '120',
  `minsecs` int(11) DEFAULT '0',
  `fu_canRecord` tinyint(1) NOT NULL DEFAULT '1',
  `maxmsg` int(11) DEFAULT '20',
  `fu_heldMaxMsg` int(11) DEFAULT '20',
  PRIMARY KEY (`uniqueid`),
  KEY `mailbox_context` (`mailbox`,`context`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `ast_voicemessages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `msgnum` int(11) NOT NULL DEFAULT '0',
  `dir` varchar(80) DEFAULT '',
  `context` varchar(80) DEFAULT '',
  `macrocontext` varchar(80) DEFAULT '',
  `callerid` varchar(40) DEFAULT '',
  `origtime` varchar(40) DEFAULT '',
  `duration` varchar(20) DEFAULT '',
  `mailboxuser` varchar(80) DEFAULT '',
  `mailboxcontext` varchar(80) DEFAULT '',
  `recording` longblob,
  `flag` varchar(128) DEFAULT '',
  `timeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `dir` (`dir`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_accessCodes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(128) DEFAULT NULL,
  `TrunkId` int(11) NOT NULL DEFAULT '0',
  `Priority` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_clis` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cliNumber` varchar(45) DEFAULT NULL,
  `cliName` varchar(45) DEFAULT NULL,
  `TrunkId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_dahdiChannels` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `TrunkId` int(11) DEFAULT NULL,
  `channelName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1$$


delimiter $$

CREATE TABLE `com_queueMembers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ParentQueueId` int(11) DEFAULT NULL,
  `Penalty` int(11) DEFAULT NULL,
  `Paused` int(11) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `ExtensionId` int(11) DEFAULT NULL,
  `QueueId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_queues` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Number` varchar(45) DEFAULT NULL,
  `Notes` text,
  `QueueName` varchar(45) DEFAULT NULL,
  `Department` varchar(45) DEFAULT NULL,
  `Strategy` enum('Ringall','Leastrecent','Fewestcalls','Random','Rrmemory','Linear','Wrandom') NOT NULL DEFAULT 'Ringall',
  `RingOnBusy` bit(1) DEFAULT NULL,
  `DDINumber` varchar(45) DEFAULT NULL,
  `VoiceMailId` int(11) DEFAULT NULL,
  `VoiceMailDelay` int(11) DEFAULT NULL,
  `CliId` int(11) NOT NULL DEFAULT '0',
  `com_musiconholdId` int(11) NOT NULL DEFAULT '0',
  `IncludeInDirectory` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_extensions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Password` varchar(45) DEFAULT NULL,
  `Notes` text,
  `Number` varchar(45) DEFAULT NULL,
  `FirstName` varchar(45) DEFAULT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `Department` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `JobTitle` varchar(45) DEFAULT NULL,
  `DDINumber` varchar(45) DEFAULT NULL,
  `VoiceMailId` int(11) DEFAULT NULL,
  `VoicemailDelay` int(11) DEFAULT NULL,
  `CliId` int(11) NOT NULL DEFAULT '0',
  `DND` tinyint(4) NOT NULL DEFAULT '0',
  `PermissionClassId` int(11) NOT NULL DEFAULT '1',
  `IncludeInDirectory` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_routingRule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `DialplanId` int(11) DEFAULT NULL,
  `Number` varchar(45) DEFAULT NULL,
  `Time` int(11) DEFAULT NULL,
  `_Order` int(11) DEFAULT NULL,
  `DestinationType` enum('Error','Extension','Group','Voicemail','External','Ringtone','Route','AutoAttendant','Playback','AddCode') NOT NULL DEFAULT 'Error',
  `DestinationNumber` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_server` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `IpAddress` varchar(45) DEFAULT NULL,
  `MailServer` varchar(45) DEFAULT NULL,
  `UserName` varchar(45) DEFAULT NULL,
  `Password` varchar(45) DEFAULT NULL,
  `voicemailNumber` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_sipTrunkCredentials` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `trunkId` int(11) NOT NULL DEFAULT '0',
  `UserName` varchar(45) DEFAULT NULL,
  `Password` varchar(45) DEFAULT NULL,
  `Host` varchar(45) DEFAULT NULL,
  `Channels` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `com_trunk` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `TrunkName` varchar(45) DEFAULT NULL,
  `TrunkType` enum('Sip','Bri','Iax') NOT NULL,
  `DefaultDestination` varchar(45) NOT NULL,
  `CLIPresentationType1` enum('StartWith','EndWith','RegExp','None') NOT NULL DEFAULT 'None',
  `CLIPresentaionValue1` varchar(100) NOT NULL,
  `CLIPresentationType2` enum('StartWith','EndWith','RegExp','None') NOT NULL DEFAULT 'None',
  `CLIPresentaionValue2` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_AutoAttendant` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `announcement` varchar(255) DEFAULT NULL,
  `timeout` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_AutoAttendantRules` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `AAName` varchar(255) DEFAULT NULL,
  `entry` varchar(45) DEFAULT NULL,
  `destination` varchar(255) DEFAULT NULL,
  `destinationType` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_contactDetails` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_firstName` varchar(45) DEFAULT NULL,
  `fu_lastName` varchar(45) DEFAULT NULL,
  `fu_department` varchar(45) DEFAULT NULL,
  `fu_email` varchar(100) DEFAULT NULL,
  `fu_jobTitle` varchar(45) DEFAULT NULL,
  `fu_extension` varchar(128) DEFAULT NULL,
  `fu_notes` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_currentDialplan` (
  `currentDialplan` int(11) NOT NULL DEFAULT '1',
  `automaticallyChange` tinyint(1) NOT NULL DEFAULT '1',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_ddis` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ddi` varchar(45) DEFAULT NULL,
  `TrunkId` int(11) NOT NULL DEFAULT '0',
  `UsedOn` enum('NotUsed','Extension','Queue','Rule','Default') NOT NULL DEFAULT 'NotUsed',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ddi_UNIQUE` (`ddi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_defaults` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_type` varchar(45) DEFAULT NULL,
  `fu_columnIndex` int(11) DEFAULT NULL,
  `fu_columnType` varchar(45) DEFAULT NULL,
  `fu_columnTitle` varchar(45) DEFAULT NULL,
  `fu_property` varchar(255) DEFAULT NULL,
  `fu_default` varchar(45) NOT NULL,
  `fu_picker` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_dialplanDates` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_dialplansId` int(11) DEFAULT NULL,
  `startDate` date DEFAULT NULL,
  `endDate` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_dialplanRanges` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `daysOfWeek` varchar(255) DEFAULT NULL,
  `timeRange` varchar(255) DEFAULT NULL,
  `priority` int(11) DEFAULT NULL,
  `fu_dialplansId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_dialplans` (
  `id` int(11) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_ringtones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL DEFAULT '',
  `sipheader` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_userConfig` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fu_password` varchar(45) DEFAULT NULL,
  `ast_extensions_exten` varchar(20) DEFAULT NULL,
  `fu_role` varchar(45) NOT NULL DEFAULT 'user',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `fu_IaxCredentials` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `host` varchar(50) DEFAULT NULL,
  `allowedChannels` int(11) NOT NULL,
  `com_trunkId` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE ALGORITHM=UNDEFINED DEFINER=`asterisk`@`%` SQL SECURITY DEFINER VIEW `ast_queue_members` AS select `com_queueMembers`.`id` AS `uniqueid`,(case `com_queueMembers`.`Type` when 1 then concat('SIP/',`extension`.`Number`) when 2 then concat('local/',`queue`.`Number`,'@LocalSets') else '' end) AS `membername`,`parent`.`Number` AS `queue_name`,(case `com_queueMembers`.`Type` when 1 then concat('SIP/',`extension`.`Number`) when 2 then concat('local/',`queue`.`Number`,'@LocalSets') else '' end) AS `interface`,`com_queueMembers`.`Penalty` AS `penalty`,`com_queueMembers`.`Paused` AS `paused` from (((`com_queueMembers` left join `com_extensions` `extension` on((`com_queueMembers`.`ExtensionId` = `extension`.`id`))) left join `com_queues` `parent` on((`com_queueMembers`.`ParentQueueId` = `parent`.`id`))) left join `com_queues` `queue` on((`com_queueMembers`.`QueueId` = `queue`.`id`)))$$

delimiter $$

CREATE ALGORITHM=UNDEFINED DEFINER=`asterisk`@`%` SQL SECURITY DEFINER VIEW `ast_queues` AS select `com_queues`.`id` AS `id`,`com_queues`.`Number` AS `name`,`com_musiconhold`.`name` AS `musiconhold`,'' AS `announce`,'' AS `context`,'' AS `timeout`,'' AS `monitor_join`,'' AS `monitor_format`,'' AS `queue_youarenext`,'' AS `queue_thereare`,'' AS `queue_callswaiting`,'' AS `queue_holdtime`,'' AS `queue_minutes`,'' AS `queue_seconds`,'' AS `queue_lessthan`,'' AS `queue_thankyou`,'' AS `queue_reporthold`,'' AS `announce_frequency`,'' AS `announce_round_seconds`,'' AS `announce_holdtime`,'' AS `retry`,'' AS `wrapuptime`,'' AS `maxlen`,'' AS `servicelevel`,lcase(`com_queues`.`Strategy`) AS `strategy`,(case `com_queues`.`RingOnBusy` when 1 then 'paused,inuse' else 'paused' end) AS `joinempty`,'' AS `leavewhenempty`,'' AS `eventmemberstatus`,'' AS `eventwhencalled`,'' AS `reportholdtime`,'' AS `memberdelay`,'' AS `weight`,'' AS `timeoutrestart`,'' AS `periodic_announce`,'' AS `periodic_announce_frequency`,'' AS `ringinuse`,'' AS `setinterfacevar` from (`com_queues` join `com_musiconhold` on((`com_queues`.`com_musiconholdId` = `com_musiconhold`.`id`)))$$

delimiter $$

CREATE ALGORITHM=UNDEFINED DEFINER=`asterisk`@`%` SQL SECURITY DEFINER VIEW `ast_sipfriends` AS select `com_extensions`.`id` AS `id`,'friend' AS `type`,`com_extensions`.`Number` AS `name`,`com_extensions`.`Password` AS `secret`,'LocalSets' AS `context`,'Dynamic' AS `host`,`com_extensions`.`Number` AS `defaultuser`,'' AS `insecure`,'BLF_Group_1' AS `subscribecontext`,'1' AS `callgroup`,'1' AS `pickupgroup`,'yes' AS `allowsubscribe`,'yes' AS `notifyringing`,'yes' AS `notifyhold`,'yes' AS `notifycid`,'' AS `port`,`ast_voicemail`.`mailbox` AS `mailbox`,(select `com_server`.`voicemailNumber` from `com_server` limit 0,1) AS `vmexten` from (`com_extensions` left join `ast_voicemail` on((`com_extensions`.`VoiceMailId` = `ast_voicemail`.`uniqueid`))) where (`com_extensions`.`Number` <> 'balls') union select `com_extensions`.`id` AS `id`,'friend' AS `type`,`com_extensions`.`Number` AS `name`,`com_extensions`.`Password` AS `secret`,'Trunks' AS `context`,'Dynamic' AS `host`,`com_extensions`.`Number` AS `defaultuser`,'' AS `insecure`,'BLF_Group_1' AS `subscribecontext`,'1' AS `callgroup`,'1' AS `pickupgroup`,'yes' AS `allowsubscribe`,'yes' AS `notifyringing`,'yes' AS `notifyhold`,'yes' AS `notifycid`,'' AS `port`,'' AS `mailbox`,'' AS `vmexten` from `com_extensions` where (`com_extensions`.`Number` = 'balls') union select `com_trunk`.`id` AS `id`,'peer' AS `peer`,`com_trunk`.`TrunkName` AS `name`,`com_sipTrunkCredentials`.`Password` AS `secret`,'Trunks' AS `context`,`com_sipTrunkCredentials`.`Host` AS `host`,`com_sipTrunkCredentials`.`UserName` AS `defaultuser`,'invite' AS `insecure`,'' AS `subscribecontext`,'' AS `callgroup`,'' AS `pickupgroup`,'' AS `allowsubscribe`,'' AS `notifyringing`,'' AS `notifyhold`,'' AS `notifycid`,'' AS `port`,'' AS `mailbox`,'' AS `vmexten` from (`com_sipTrunkCredentials` left join `com_trunk` on((`com_sipTrunkCredentials`.`trunkId` = `com_trunk`.`id`)))$$

delimiter $$

CREATE ALGORITHM=UNDEFINED DEFINER=`asterisk`@`%` SQL SECURITY DEFINER VIEW `com_iaxFriends` AS select `fu_IaxCredentials`.`name` AS `name`,`fu_IaxCredentials`.`host` AS `host`,'' AS `port`,'' AS `ipaddr`,'' AS `secret`,`fu_IaxCredentials`.`name` AS `peername`,`fu_IaxCredentials`.`name` AS `username`,`fu_IaxCredentials`.`com_trunkId` AS `com_trunkId`,'friend' AS `type`,'yes' AS `trunk`,'incoming' AS `context`,'' AS `regseconds` from `fu_IaxCredentials`$$



