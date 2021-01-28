import BaseService from './BaseService';
import { ICustomer } from '../common/types/ICustomer';

class CustomerManagementService extends BaseService {

  async deleteCustomer(customerId: number): Promise<void> {
    return await this.delete(`customer/${customerId}`);
  }

  async getCustomers(): Promise<ICustomer[]> {
    return await this.get(`customer`);
  }

  async getDuplicateCustomers(): Promise<ICustomer[]> {
    var duplicatedList: ICustomer[] = await this.get(`customer/duplicate`);

    const aggregatedCustomerList = this.aggregateDubplicateCustomers(duplicatedList);

    return aggregatedCustomerList;
  }

  async editCustomer(customer: ICustomer): Promise<ICustomer> {
    return await this.put(`customer`, customer);
  }

  async resolveCustomerName(customer: ICustomer): Promise<ICustomer[]> {
    return await this.put(`customer/resolvename`, customer);

  }

  async addCustomer(customer: ICustomer): Promise<ICustomer[]> {
    return await this.post(`customer`, customer);
  }

  groupCustomers(list: ICustomer[], keyGetter: any) {
    const map = new Map();

    list.forEach((cust: ICustomer) => {
      const key = keyGetter(cust);
      const collection = map.get(key);
      if (!collection) {
        cust.duplicateCount = 0;
        map.set(key, [cust]);
      } else {
        cust.duplicateCount = 0;
        collection.push(cust);
      }
    });
    return map;
  }



  aggregateDubplicateCustomers(list: ICustomer[]) {

    var sortedList = list.sort((a, b) => a.firstName.localeCompare(b.firstName) || a.lastName.localeCompare(b.lastName) || b.email.localeCompare(a.email));

    const grouped = this.groupCustomers(sortedList, (cust: ICustomer) => cust.firstName + ' - ' + cust.lastName);

    var aggregatedCustomerList: ICustomer[] = [];

    grouped.forEach(g => {

      var topCustomer: ICustomer = {
        id: g[0].id,
        firstName: g[0].firstName,
        lastName: g[0].lastName,
        email: g[0].email,
        phoneNumber: g[0].phoneNumber,
        duplicateCount: g.length
      };

      aggregatedCustomerList.push(topCustomer);

    })
    return aggregatedCustomerList;
  }

}



const service = new CustomerManagementService();

export default service;
