drop function if exists public.buy_sell_stock();
CREATE OR REPLACE FUNCTION public.buy_sell_stock()
	RETURNS void 
	LANGUAGE plpgsql
AS $function$
    /**
     * Hàm thực hiện các lệnh mua bán chứng khoán của người dùng
     * @author nttue - 02.11.2024
     */
    BEGIN   
    /**
     * Bước 1: Lấy ra các lệnh mua và các lệnh bán của chứng khoán trên sàn giao dịch của người dùng     
     */       
       drop table if exists tmp_buy_sell_stock;
        create temp table tmp_buy_sell_stock(
            transaction_id uuid,
            stock_id uuid, 
            user_id uuid,
            price decimal(10, 2), 
            volume int,
            transaction_type int
        );

        -- Lấy ra các lệnh mua bán
        insert into tmp_buy_sell_stock
        select 
            transaction_id,
            stock_id,
            user_id, 
            order_price as price, 
            volume, 
            transaction_type
        from transactions
        order by created_at desc;

        -- Lấy thêm các lệnh bán của mã chứng khoán
        insert into tmp_buy_sell_stock
        select 
            null as transaction_id,
            stock_id,
            null as user_id, 
            matched_price as price, 
            tradable_volume as volume, 
            1 as transaction_type
        from stocks
        where tradable_volume > 0;

        
        /**
         * Bước 2: Thực hiện khớp các lệnh mua bán chứng khoán của người dùng
         */
        with v_temp as (

        )

    end;
$function$
                           