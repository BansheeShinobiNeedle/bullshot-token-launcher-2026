# Developer Notes

## Overview

Bullshot Token Launcher is a desktop companion for Bullshot — the memecoin launch platform on BNB Chain. It allows users to track Bonding Curve progress, calculate creator fees, and manage quests.

The utility is strictly read-only. It does not interact with wallets, sign transactions, or require any private keys.

## Supported Chains

Bullshot supports multiple chains:
- BNB Chain
- Robinhood Chain
- Base
- ARC Chain

## Data Source

Data is aggregated from public Bullshot sources. No API keys are required for the stub implementation.

## Updating the Endpoint

The BondingCurveTracker class contains TODO comments where real public API endpoints should be inserted. Replace the stub URLs with actual public endpoints as needed.

## Known Limitations

- Figures may differ between sources.
- Data is aggregated from public dashboards and may have reporting delays.
- Accuracy depends on user input and public data availability.