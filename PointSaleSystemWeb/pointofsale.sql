create table account
(
	account_id int auto_increment
		primary key,
	user_id int not null,
	username varchar(20) not null,
	password varchar(32) not null,
	role_id int not null,
	status bit default b'1' not null,
	constraint account_username_uindex
		unique (username)
)
;

create index account_role_role_id_fk
	on account (role_id)
;

create index account_user_user_id_fk
	on account (user_id)
;

create table administrator
(
	admin_id int auto_increment
		primary key,
	full_name varchar(100) null,
	username varchar(20) null,
	password varchar(32) null,
	picture longblob null
)
;

create table category
(
	category_id int auto_increment
		primary key,
	category varchar(50) not null,
	expire bit default b'1' not null
)
;

create table complaint
(
	complaint_id int auto_increment
		primary key,
	complaint varchar(200) not null
)
;

create table csv
(
	PRODUCT_ID int auto_increment
		primary key,
	COST_PRICE double null,
	CATEGORY_ID int null,
	PRODUCT_NAME text null,
	SELLING_PRICE int null,
	constraint CSV_PRODUCT_ID_uindex
		unique (PRODUCT_ID)
)
;

create table customer
(
	customer_id int auto_increment
		primary key,
	customer_name varchar(100) not null,
	customer_phone varchar(20) not null
)
;

create table list_item
(
	list_item_id int auto_increment
		primary key,
	order_id int null,
	order_number varchar(20) null,
	product_id int not null,
	quantity_sold int not null,
	price varchar(20) null
)
;

create index list_item_order_order_id_order_number_fk
	on list_item (order_id, order_number)
;

create index list_item_product_product_id_fk
	on list_item (product_id)
;

create trigger list_item_AFTER_INSERT
             after INSERT on list_item
             for each row
begin
		update
			product set
				quantity_available = product.quantity_available - new.quantity_sold
			where
				product.product_id = new.product_id;
end;

create trigger list_item_AFTER_DELETE
             after DELETE on list_item
             for each row
begin
		update
			product set
				quantity_available = product.quantity_available + OLD.quantity_sold
			where
				product.product_id = OLD.product_id;
end;

create table `order`
(
	order_id int auto_increment,
	order_number varchar(20) not null,
	order_date date not null,
	total varchar(20) null,
	customer_id int null,
	user_id int not null,
	primary key (order_id, order_number),
	constraint order_order_id_uindex
		unique (order_id),
	constraint order_cart_no_uindex
		unique (order_number),
	constraint order_customer_customer_id_fk
		foreign key (customer_id) references customer (customer_id)
)
;

create index FK__SALE__UserID__76619304
	on `order` (user_id)
;

create index order_customer_customer_id_fk
	on `order` (customer_id)
;

alter table list_item
	add constraint list_item_order_order_id_order_number_fk
		foreign key (order_id, order_number) references `order` (order_id, order_number)
			on update set null on delete set null
;

create table password_reset
(
	password_reset_id int auto_increment
		primary key,
	user_id int not null,
	status char default '1' not null
)
;

create index FK__PASSWORDR__UserI__66603565
	on password_reset (user_id)
;

create table product
(
	product_id int auto_increment
		primary key,
	product_name varchar(50) null,
	category_id int not null,
	quantity_available int default '0' not null,
	cost_price decimal(4,2) not null,
	status bit default b'1' null,
	min_stock_level int null,
	selling_price decimal(4,2) not null,
	constraint fk_category
		foreign key (category_id) references category (category_id)
)
;

create index RefCATEGORY1
	on product (category_id)
;

alter table list_item
	add constraint list_item_product_product_id_fk
		foreign key (product_id) references product (product_id)
;

create table product_inventory
(
	inventory_id int auto_increment
		primary key,
	inventory_date date not null,
	product_id int not null,
	quantity_delivered int not null,
	batch_number varchar(20) null,
	product_expiry_date date null,
	user_id int not null,
	status bit default b'0' null,
	constraint product_inventory_product_product_id_fk
		foreign key (product_id) references product (product_id)
)
;

create index RefSTOCK2
	on product_inventory (product_id)
;

create index RefUSER4
	on product_inventory (user_id)
;

create trigger product_inventory_AFTER_INSERT
             after INSERT on product_inventory
             for each row
begin
		update
			product set
				quantity_available = product.quantity_available + new.quantity_delivered
			where
				product.product_id = new.product_id;
end;

create table product_return
(
	product_return_id int auto_increment
		primary key,
	product_id int not null,
	quantity_returned int not null,
	dat_of_return date not null,
	complaint_id int not null,
	user_id int not null
)
;

create index fk_complaint
	on product_return (complaint_id)
;

create table role
(
	role_id int auto_increment
		primary key,
	role varchar(20) not null
)
;

alter table account
	add constraint account_role_role_id_fk
		foreign key (role_id) references role (role_id)
;

create table setting
(
	setting_id int auto_increment
		primary key,
	shop_name varchar(100) not null,
	contact_number varchar(10) not null,
	address varchar(100) null,
	email varchar(100) null,
	website varchar(100) null
)
;

create table user
(
	user_id int auto_increment
		primary key,
	first_name varchar(20) not null,
	last_name varchar(20) not null,
	gender varchar(7) not null,
	date_of_birth date not null,
	date_of_employment date null,
	role_id int not null,
	phone_number varchar(15) not null,
	address varchar(100) null,
	picture longblob null,
	status bit default b'1' null,
	constraint user_role_role_id_fk
		foreign key (role_id) references role (role_id)
)
;

create index RefROLE6
	on user (role_id)
;

alter table account
	add constraint account_user_user_id_fk
		foreign key (user_id) references user (user_id)
			on update cascade on delete cascade
;

alter table `order`
	add constraint order_user_user_id_fk
		foreign key (user_id) references user (user_id)
;

alter table product_inventory
	add constraint product_inventory_user_user_id_fk
		foreign key (user_id) references user (user_id)
;

create table user_log
(
	user_log_id int auto_increment
		primary key,
	user_id int not null,
	role_id int not null,
	date date not null,
	login_time datetime null,
	logout_time datetime null,
	constraint user_log_user_user_id_fk
		foreign key (user_id) references user (user_id),
	constraint user_log_role_role_id_fk
		foreign key (role_id) references role (role_id)
)
;

create index user_log_role_role_id_fk
	on user_log (role_id)
;

create index user_log_user_user_id_fk
	on user_log (user_id)
;

create view inventory_view as 
SELECT
    `b`.`product_name`                             AS `product_name`,
    `a`.`inventory_date`                           AS `inventory_date`,
    `a`.`quantity_delivered`                       AS `quantity_delivered`,
    `a`.`batch_number`                             AS `batch_number`,
    `a`.`product_expiry_date`                      AS `product_expiry_date`,
    concat(`c`.`first_name`, ' ', `c`.`last_name`) AS `full_name`,
    `a`.`status`                                   AS `status`
  FROM ((`pos`.`product_inventory` `a` LEFT JOIN `pos`.`product` `b` ON ((`a`.`product_id` = `b`.`product_id`))) JOIN
    `pos`.`user` `c` ON ((`a`.`user_id` = `c`.`user_id`)));

create view order_view as 
SELECT
    `a`.`list_item_id`                             AS `list_item_id`,
    `a`.`order_id`                                 AS `order_id`,
    `a`.`order_number`                             AS `order_number`,
    `b`.`order_date`                               AS `order_date`,
    `c`.`product_id`                               AS `product_id`,
    `c`.`product_name`                             AS `product_name`,
    `a`.`quantity_sold`                            AS `quantity_sold`,
    `a`.`price`                                    AS `price`,
    `d`.`customer_name`                            AS `customer_name`,
    concat(`e`.`first_name`, ' ', `e`.`last_name`) AS `user_name`
  FROM ((((`pos`.`list_item` `a` LEFT JOIN `pos`.`order` `b` ON ((`b`.`order_id` = `a`.`order_id`))) LEFT JOIN
    `pos`.`product` `c` ON ((`c`.`product_id` = `a`.`product_id`))) LEFT JOIN `pos`.`customer` `d`
      ON ((`d`.`customer_id` = `b`.`customer_id`))) JOIN `pos`.`user` `e` ON ((`e`.`user_id` = `b`.`user_id`)));

create view product_view as 
SELECT
    `a`.`product_id`         AS `product_id`,
    `a`.`product_name`       AS `product_name`,
    `b`.`category`           AS `category`,
    `a`.`quantity_available` AS `quantity_available`,
    `a`.`cost_price`         AS `cost_price`,
    `a`.`selling_price`      AS `selling_price`,
    `a`.`min_stock_level`    AS `min_stock_level`,
    `a`.`status`             AS `status`
  FROM (`pos`.`product` `a` LEFT JOIN `pos`.`category` `b` ON ((`a`.`category_id` = `b`.`category_id`)));

create view user_log_view as 
SELECT
    `a`.`user_id`                                  AS `user_id`,
    concat(`c`.`first_name`, ' ', `c`.`last_name`) AS `full_name`,
    `b`.`role`                                     AS `role`,
    `a`.`date`                                     AS `date`,
    `a`.`login_time`                               AS `login_time`,
    `a`.`logout_time`                              AS `logout_time`
  FROM ((`pos`.`user_log` `a`
    JOIN `pos`.`role` `b` ON ((`b`.`role_id` = `a`.`role_id`))) JOIN `pos`.`user` `c`
      ON ((`a`.`user_id` = `c`.`user_id`)));

create view user_view as 
SELECT
    `a`.`user_id`                                  AS `user_id`,
    concat(`a`.`first_name`, ' ', `a`.`last_name`) AS `full_name`,
    `a`.`gender`                                   AS `gender`,
    `a`.`date_of_birth`                            AS `date_of_birth`,
    `a`.`date_of_employment`                       AS `date_of_employment`,
    `b`.`role`                                     AS `role`,
    `a`.`phone_number`                             AS `phone_number`,
    `a`.`address`                                  AS `address`,
    `c`.`username`                                 AS `username`,
    `a`.`picture`                                  AS `picture`,
    `a`.`status`                                   AS `status`
  FROM ((`pos`.`user` `a`
    JOIN `pos`.`role` `b` ON ((`b`.`role_id` = `a`.`role_id`))) JOIN `pos`.`account` `c`
      ON ((`a`.`user_id` = `c`.`user_id`)));

