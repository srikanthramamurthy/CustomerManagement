import React from "react";
import "./Home.css";
import { ICustomer } from "../common/types/ICustomer";
import CustomerManagementService from "../services/CustomerManagementService";
import CustomerList from "../Customers/CustomerList";
import NavBar from "../Component/navbar/NavBar";
import Spinner from "../Component/UI/Spinner";
import { Container } from "@material-ui/core";
import { ViewMode } from "../common/types/enums";
import Button from "@material-ui/core/Button";
import Snackbar, { SnackbarOrigin } from "@material-ui/core/Snackbar";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import SnackbarContent from "@material-ui/core/SnackbarContent";
import UnprocessableEntity from "../services/errors/UnprocessableEntity";

interface IState {
  customers: ICustomer[] | undefined;
  loader: Boolean | undefined;
  error: Boolean;
  message: string;
  viewMode: ViewMode;
  showPopup: boolean;
  openSnackbar: boolean;
}

class HomeContainer extends React.Component {
  readonly state: Readonly<IState> = {
    customers: undefined,
    loader: false,
    error: false,
    viewMode: ViewMode.AllCustomers,
    showPopup: false,
    openSnackbar: false,
    message: "",
  };

  private _isMounted = false;

  async componentDidMount() {
    this._isMounted = true;
    await this.loadCustomers(this.state.viewMode);
  }

  componentWillUnmount() {
    this._isMounted = false;
  }

  handleCloseSnackbar = () => {
    this.setState({ openSnackbar: false });
  };

  async loadCustomers(viewMode: ViewMode) {
    try {
      this.updateState({ loader: true });
      await new Promise((resolve) => setTimeout(resolve, 1000));

      const customers =
        viewMode === ViewMode.AllCustomers
          ? await CustomerManagementService.getCustomers()
          : await CustomerManagementService.getDuplicateCustomers();

      this.updateState({
        loader: false,
        error: false,
        customers: customers,
        viewMode,
      });
    } catch (e) {
      this.updateState({ loader: false, error: true, message: e.message });
    }
  }

  reLoadCustomers = async (viewMode: ViewMode) => {
    try {
      this.loadCustomers(viewMode);
    } catch (e) {
      this.updateState({ loader: false, error: true });
    }
  };

  deleteCustomer = async (customerId: number, viewMode: ViewMode) => {
    try {
      this.updateState({ loader: true });
      await CustomerManagementService.deleteCustomer(customerId);
      await this.loadCustomers(viewMode);
      this.updateState({ showPopup: false });
    } catch (e) {
      this.updateState({
        loader: false,
        showPopup: false,
        error: true,
        message: e.message,
      });
    }
  };

  addCustomer = async (customer: ICustomer, viewMode: ViewMode) => {
    try {
      await CustomerManagementService.addCustomer(customer);
      await this.loadCustomers(viewMode);
      this.updateState({ showPopup: false });
    } catch (e) {
      this.updateState({
        loader: false,
        error: true,
        openSnackbar: true,
        message: e.message,
      });
    }
  };

  editCustomer = async (customer: ICustomer, viewMode: ViewMode) => {
    try {
      if (viewMode === ViewMode.AllCustomers) {
        await CustomerManagementService.editCustomer(customer);
      } else {
        await CustomerManagementService.resolveCustomerName(customer);
      }

      await this.loadCustomers(viewMode);
      this.updateState({ showPopup: false });
    } catch (e) {
      this.updateState({
        loader: false,
        error: true,
        openSnackbar: true,
        message: e.message,
      });

      return new UnprocessableEntity(e.message);
    }
  };

  updateState(updatedState: Partial<IState>) {
    if (this._isMounted) {
      this.setState(updatedState);
    }
  }

  togglePopUp = (showPopup: boolean) => {
    this.setState({ showPopup: showPopup });
  };

  render() {
    const { customers, viewMode } = this.state;

    return (
      <div>
        <NavBar />
        {this.state.loader ? <Spinner /> : ""}

        {customers != null && !this.state.loader ? (
          <CustomerList
            customers={customers}
            viewMode={viewMode}
            togglePopup={this.togglePopUp}
            showPopup={this.state.showPopup}
            onDeleteCustomer={this.deleteCustomer}
            onAddCustomer={this.addCustomer}
            onEditCustomer={this.editCustomer}
            loadCustomers={this.reLoadCustomers}
          />
        ) : (
          ""
        )}

        {!this.state.loader && this.state.error ? (
          <Container>
            <Snackbar
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              open={this.state.openSnackbar}
              onClose={this.handleCloseSnackbar}
            >
              <SnackbarContent
                style={
                  this.state.error
                    ? { backgroundColor: "red" }
                    : { backgroundColor: "green" }
                }
                message={this.state.message}
                action={
                  <React.Fragment>
                    <IconButton
                      size="small"
                      aria-label="close"
                      color="inherit"
                      onClick={this.handleCloseSnackbar}
                    >
                      <CloseIcon fontSize="small" />
                    </IconButton>
                  </React.Fragment>
                }
              />
            </Snackbar>
          </Container>
        ) : (
          ""
        )}
      </div>
    );
  }
}

export default HomeContainer;
