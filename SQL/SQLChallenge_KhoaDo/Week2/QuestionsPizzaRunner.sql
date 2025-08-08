--A. Pizza Metrics
--How many pizzas were ordered?
SELECT
	COUNT(order_id) AS pizza_count
FROM customer_orders;

--How many unique customer orders were made?
SELECT
	COUNT(DISTINCT order_id) unique_order
FROM customer_orders;

--How many successful orders were delivered by each runner?
SELECT
	rno.runner_id,
	COUNT(rno.runner_id) successful_orders_count
FROM customer_orders cto
JOIN runner_orders rno ON rno.order_id = cto.order_id 
WHERE rno.cancellation != 'Restaurant Cancellation' AND rno.cancellation != 'Customer Cancellation'
GROUP BY rno.runner_id



--How many of each type of pizza was delivered?
--How many Vegetarian and Meatlovers were ordered by each customer?
--What was the maximum number of pizzas delivered in a single order?
--For each customer, how many delivered pizzas had at least 1 change and how many had no changes?
--How many pizzas were delivered that had both exclusions and extras?
--What was the total volume of pizzas ordered for each hour of the day?
--What was the volume of orders for each day of the week?