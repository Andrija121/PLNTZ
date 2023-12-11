import { render, screen } from "@testing-library/react";

test("renders learn react link", () => {
  const linkElement = screen.queryByText(/learn react/i);
  expect(linkElement).not.toBeInTheDocument();
});
