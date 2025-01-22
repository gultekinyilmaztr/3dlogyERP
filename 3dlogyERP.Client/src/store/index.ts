import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import orderReducer from './slices/orderSlice';
import customerReducer from './slices/customerSlice';
import expenseReducer from './slices/expenseSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    orders: orderReducer,
    customers: customerReducer,
    expenses: expenseReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
