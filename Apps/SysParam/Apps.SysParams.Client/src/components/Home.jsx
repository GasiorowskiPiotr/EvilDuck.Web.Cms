import React from 'react';
import SystemParametersStore from '../stores/SystemParametersStore.jsx';
import {Link} from 'react-router';

export default class Main extends React.Component {

  constructor() {
    super();
    this.state = {items: []};
  }

  componentDidMount() {
    SystemParametersStore.addChangeListener(() => {
      this.setState(SystemParametersStore.getItems());
    });
    SystemParametersStore.loadAll();
  }

  render() {

    var emptyContent;
    if(!this.state.item || this.state.items.length ==0) {
      emptyContent = (<tr><td colSpan="8" style={{'text-align': 'center'}}>Brak danych</td></tr>);
    }

    return (
      <div>
        <table className="table table-hover">
          <tbody>
            <tr>
              <td>Akcja</td>
              <td>Kod</td>
              <td>Typ</td>
              <td>Wartosc</td>
              <td>Utworzone przez</td>
              <td>Data utworzenia</td>
              <td>Ostatnio zmodyfikowane przez</td>
              <td>Data ostatniej modyfikacji</td>
            </tr>
            {emptyContent}
            {this.state.items.map((item) => {
              return (
                <tr>
                  <td></td>
                  <td>{item.Id}</td>
                  <td>{item.ParameterType}</td>
                  <td>{item.SerializedValue}</td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                </tr>
            );
            })}
          </tbody>
        </table>
        <hr/>
        <Link to="add">Dodaj parametr systemowy</Link>
      </div>
    );
  }
}
